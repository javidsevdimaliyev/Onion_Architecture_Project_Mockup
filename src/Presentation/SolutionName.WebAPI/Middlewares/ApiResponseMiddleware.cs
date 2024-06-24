using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using SolutionName.Application.Models.Common;
using SolutionName.Application.Enums;
using SolutionName.Application.Exceptions;
using SolutionName.Application.Utilities.Extensions;
using System.Diagnostics;
using System.Net;
using System.Text;


namespace SolutionName.WebAPI.Middlewares;

public class APIResponseMiddleware : IMiddleware
{
    private readonly ILogger<APIResponseMiddleware> _logger;

    public APIResponseMiddleware(ILogger<APIResponseMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var watch = new Stopwatch();
        watch.Start();
        if (IsSwagger(context) || IsSignalR(context) || IsHealthChecks(context))
            await next(context);

        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;
        try
        {
            await next.Invoke(context);
            watch.Stop();
            if (context.Response.StatusCode == (int)HttpStatusCode.OK)
            {
                var body = await FormatResponse(context.Response);
                await HandleSuccessRequestAsync(
                    context,
                    body,
                    context.Response.StatusCode,
                    watch.ElapsedMilliseconds);
            }
            else
            {
                await HandleNotSuccessRequestAsync(
                    context,
                    context.Response.StatusCode,
                    watch.ElapsedMilliseconds);
            }
        }
        catch (Exception ex)
        {
            watch.Stop();
            await HandleExceptionAsync(context, ex, watch.ElapsedMilliseconds);
        }
        finally
        {
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
            //responseBody.Close();
        }
    }

    #region Handlers

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, long ms)
    {
        ApiException apiexception = null;
        string details = null;
#if (DEBUG)
        details = exception.StackTrace;
#endif

        var code = 0;
        switch (exception)
        {
            case ApiException:
                apiexception = exception as ApiException;
                apiexception.Details = details;
                code = (int)apiexception.StatusCode;
                break;
            case OperationCanceledException:
                apiexception =
                    new ApiException(HttpResponseStatus.OperationCancelled.GetDescription(context.Request.Path));
                _logger.LogInformation(HttpResponseStatus.OperationCancelled.GetDescription(context.Request.Path));
                apiexception.Details = details;
                code = (int)HttpResponseStatus.OperationCancelled;
                break;
            case InvalidOperationException:
                if (exception.Message.StartsWith("No authenticationScheme"))
                {
                    apiexception = new ApiException(HttpResponseStatus.Unauthorized.GetDescription());
                    apiexception.Details = details;
                    code = (int)HttpResponseStatus.Unauthorized;
                }
                else
                {
                    apiexception = new ApiException(HttpResponseStatus.Exception);
                    apiexception.Details = details;
                    code = (int)HttpResponseStatus.Exception;
                }

                break;
            case UnauthorizedAccessException:
                apiexception = new ApiException(HttpResponseStatus.Unauthorized.GetDescription());
                apiexception.Details = details;
                var message = exception.Message?.Split(":").Last().Trim();
                code = message switch
                {
                    "ExpiredRefreshToken" => (int)HttpResponseStatus.ExpiredRefreshToken,
                    "DifferentRemoteIP" => (int)HttpResponseStatus.DifferentRemoteIP,
                    _ => (int)HttpResponseStatus.Unauthorized
                };
                break;
            default:
                var msg = exception.GetBaseException().Message;
                apiexception = new ApiException(msg);
                apiexception.Details = details;
                code = (int)HttpResponseStatus.Exception;
                break;
        }

        context.Response.ContentType = "application/json";
        var apiResponse = new APIResponse(
            code,
            HttpResponseStatus.Exception.GetDescription(),
            null,
            apiexception,
            "",
            ms.ToString());
        var json = apiResponse.GetSerialized();
        await context.Response.WriteAsync(json);
        await AfterExecuted(LogLevel.Critical, context, exception, ms);
    }

    private async Task HandleNotSuccessRequestAsync(HttpContext context, int code, long ms)
    {
        if (context.Request.Method != "OPTIONS")
        {
            context.Response.ContentType = "application/json";

            ApiException apiexception = null;
            APIResponse apiResponse = null;

            apiexception = code switch
            {
                (int)HttpStatusCode.NotFound => new ApiException(HttpResponseStatus.NotFound.GetDescription()),
                (int)HttpStatusCode.NoContent => new ApiException(HttpResponseStatus.NoContent.GetDescription()),
                (int)HttpStatusCode.Unauthorized => new ApiException(
                    HttpResponseStatus.Unauthorized.GetDescription()),
                _ => new ApiException(HttpResponseStatus.Exception.GetDescription())
            };

            apiResponse = new APIResponse(code, HttpResponseStatus.Failure.GetDescription(), null, apiexception,
                "1.0.0.0", ms.ToString());
            //   context.Response.StatusCode = code;
            var json = apiResponse.GetSerialized();

            await context.Response.WriteAsync(json);
            await AfterExecuted(LogLevel.Error, context, null, ms);
        }
    }

    private async Task HandleSuccessRequestAsync(HttpContext context, object body, int code, long ms)
    {
        context.Response.ContentType = "application/json";
        string jsonString = "", bodyText;
        if (!body.ToString().IsValidJson())
            bodyText = body.GetSerialized();
        else
            bodyText = body.ToString();

        var bodyContent = bodyText.GetDeserialized<object>();
        var type = bodyContent?.GetType();


        if (type == typeof(APIResponse) || typeof(APIResponse).IsAssignableFrom(type))
        {
            var resp = (APIResponse)bodyContent;
            resp.ExecutingTime = ms.ToString();
            jsonString = resp.GetSerialized();
        }
        else
        {
            var apiResponse = new APIResponse(code, HttpResponseStatus.Success.GetDescription(), bodyContent, null,
                "", ms.ToString());
            jsonString = apiResponse.GetSerialized();
        }

        var array = Encoding.UTF8.GetBytes(jsonString);
        await context.Response.Body.WriteAsync(array, 0, array.Length);
        context.Response.Body.SetLength(array.LongLength);

        var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
        //var auditAttribute = endpoint?.Metadata.GetMetadata<AuditLogAttribute>();
        //if (auditAttribute != null) await AfterExecuted(LogLevel.Information, context, null, ms);
    }

    #endregion

    #region HelpMethods

    private async Task AfterExecuted(LogLevel logLevel, HttpContext httpContext, Exception exception = null,
        double ms = 0)
    {
        //var endpoint = httpContext.Features.Get<IEndpointFeature>()?.Endpoint; 
        //var notLogAttribute = endpoint?.Metadata.GetMetadata<NotLogAttribute>();
        //if ( notLogAttribute == null)
        //{
        //    // var httpContext = context.HttpContext;
        //    var request = httpContext.Request;
        //    var response = httpContext.Response;
        //    var auditAction = new AuditAction()
        //    {
        //        User = httpContext.User?.Identity.Name ?? IdentityWork.GetId().ToString(),
        //        IpAddress = httpContext.Connection?.RemoteIpAddress?.ToString(),
        //        RequestUrl = string.Format("{0}://{1}{2}{3}", request.Scheme, request.Host, request.Path,
        //            request.QueryString),
        //        HttpMethod = request.Method,
        //        FormVariables = request.HasFormContentType ? ToDictionary(request.Form) : null,
        //        RequestBody = new BodyContent
        //        {
        //            Type = request.ContentType, Length = request.ContentLength,
        //            Value = ReadRequestBodyExtension.GetRequestBody(httpContext)
        //        },
        //        ResponseBody = new BodyContent
        //        {
        //            Type = response.ContentType, Length = response.ContentLength,
        //            Value = ReadRequestBodyExtension.GetResponseBody(httpContext)
        //        },
        //        //ActionName = ActionDescriptior?.ActionName ?? ActionDescriptior?.DisplayName,
        //        //  ControllerName = ActionDescriptior?.ControllerName,
        //        TraceId = httpContext.TraceIdentifier,
        //        TotalMilliseconds = ms,
        //        Type = LogType.AuditLog,
        //        StatusCode = Convert.ToInt32(httpContext.Response?.StatusCode),
        //        UserAgent = request.UserAgentInfo(),
        //        Exception = exception
        //    };
        //    _log.Log(logLevel, exception, "{UserId} {Body}", auditAction.User, auditAction.GetSerializeObject());
        //}
    }

    private async Task<string> FormatResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var plainBodyText = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        return plainBodyText;
    }

    private bool IsSwagger(HttpContext context)
    {
        return context.Request.Path.StartsWithSegments("/swagger");
    }

    private bool IsHealthChecks(HttpContext context)
    {
        return context.Request.Path.StartsWithSegments("/healthchecks-ui");
    }

    private bool IsSignalR(HttpContext context)
    {
        return context.Request.Path.StartsWithSegments("/Hub");
    }

    private static IDictionary<string, string> ToDictionary(IEnumerable<KeyValuePair<string, StringValues>> col)
    {
        if (col == null) return null;

        IDictionary<string, string> dict = new Dictionary<string, string>();
        foreach (var k in col) dict.Add(k.Key, string.Join(", ", k.Value));

        return dict;
    }

    #endregion

   
}