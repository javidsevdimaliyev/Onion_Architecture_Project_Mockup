using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using SolutionName.Application.Enums;
using SolutionName.Application.Exceptions;
using System.Net;

namespace SolutionName.WebAPI.Filters
{
    public class GlobalApiExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// Default olaraq ancaq unhandled exceptionlar loglayiriq
        /// </summary>
        public void OnException(Microsoft.AspNetCore.Mvc.Filters.ExceptionContext context)
        {
            // Custom exceptionlar Status və Message olaraq tutulacaq, main Request statusu InternalServerError(500) və detaildəki Status isə dinamik olaraq göndəriləcək
            if (context.Exception is ApiException apiException)
            {
                HandleApiExceptions(context, apiException);
            }
            // CancellationToken ilə sonlandırılış əməliyyatlar üçün
            else if (context.Exception is OperationCanceledException)
            {
                HandleCancellationTokenException(context);
            }
            else if (context.Exception is DbUpdateException dbUpdateException)
            {
                HandleSqlExceptions(context, dbUpdateException);
            }
            // Unhandled exceptionlar isə Internal Server Error olaraq göndəriləcək
            else
            {
                UnHandledException(context);
            }
            context.ExceptionHandled = true;
        }

        private void HandleApiExceptions(Microsoft.AspNetCore.Mvc.Filters.ExceptionContext context, ApiException apiException)
        {
            context.Result = new ObjectResult(new
            {
                apiException.Status,
                apiException.Message
            });
            context.HttpContext.Response.StatusCode = apiException.Status == HttpResponseStatus.Unauthorized ? (int)HttpStatusCode.Unauthorized : (int)HttpStatusCode.BadRequest;

            //Log.Warning(apiException.Message);
        }

        private void HandleCancellationTokenException(Microsoft.AspNetCore.Mvc.Filters.ExceptionContext context)
        {
            var canceledException = new ApiException(HttpResponseStatus.OperationCancelled);
            context.Result = new ObjectResult(new
            {
                canceledException.Status,
                canceledException.Message
            });

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }

        private void HandleSqlExceptions(Microsoft.AspNetCore.Mvc.Filters.ExceptionContext context, DbUpdateException dbUpdateException)
        {
            if (context.Exception?.InnerException is null)
                UnHandledException(context);
            ApiException apiException = new(HttpResponseStatus.DbException);

            if (dbUpdateException!.InnerException!.Message.Contains("FOREIGN KEY"))
            {
                apiException = new ApiException(HttpResponseStatus.ForeignKeyException);
            }
            else if (dbUpdateException.InnerException.Message.Contains("UNIQUE KEY"))
            {
                apiException = new ApiException(HttpResponseStatus.DuplicateKeyException);
            }
            else if (dbUpdateException.InnerException.Message.Contains("REFERENCE constraint"))
            {
                apiException = new ApiException(HttpResponseStatus.ReferfenceConstraintException);
            }
            context.Result = new ObjectResult(new
            {
                apiException.Status,
                apiException.Message
            });
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            ////Log.Error(context.Exception?.InnerException, context.Exception?.InnerException?.Message);
        }

        private void UnHandledException(Microsoft.AspNetCore.Mvc.Filters.ExceptionContext context)
        {
            context.Result = new ObjectResult(new
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Message = context.Exception?.Message ?? context.Exception?.InnerException?.Message,
                //#if DEBUG 
                ExceptionDetails = context.Exception?.StackTrace
                //#endif
            });
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            ////Log.Error(context.Exception, "UnHandledException");
        }
    }
}
