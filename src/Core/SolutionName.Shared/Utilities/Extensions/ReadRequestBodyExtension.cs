using System.Text;
using Microsoft.AspNetCore.Http;

namespace SolutionName.Application.Shared.Utilities.Extensions;

public static class ReadRequestBodyExtension
{
    public static async Task<string> GetRawBodyAsync(this HttpRequest request, Encoding encoding = null)
    {
        if (!request.Body.CanSeek)
            // We only do this if the stream isn't *already* seekable,
            // as EnableBuffering will create a new stream instance
            // each time it's called
            request.EnableBuffering();

        request.Body.Position = 0;

        var reader = new StreamReader(request.Body, encoding ?? Encoding.UTF8);

        var body = await reader.ReadToEndAsync().ConfigureAwait(false);

        request.Body.Position = 0;

        return body;
    }

    //Wep Api
    public static string GetRequestBody(HttpContext httpContext)
    {
        var body = httpContext.Request.Body;
        if (body != null && body.CanRead && body.CanSeek)
            using (var stream = new MemoryStream())
            {
                if (body.CanSeek) body.Seek(0, SeekOrigin.Begin);
                body.CopyTo(stream);
                if (body.CanSeek) body.Seek(0, SeekOrigin.Begin);


                return Encoding.UTF8.GetString(stream.ToArray()).Replace();
            }

        return ReadRequestFormBodyAsString(httpContext);
    }


    public static string GetResponseBody(HttpContext httpContext)
    {
        var body = httpContext.Response.Body;
        if (body != null && body.CanRead && body.CanSeek)
            using (var stream = new MemoryStream())
            {
                if (body.CanSeek) body.Seek(0, SeekOrigin.Begin);
                body.CopyTo(stream);
                if (body.CanSeek) body.Seek(0, SeekOrigin.Begin);


                return Encoding.UTF8.GetString(stream.ToArray()).Replace();
            }

        return "";
    }

    //Mvc App
    public static string ReadRequestFormBodyAsString(HttpContext httpContext)
    {
        var form = httpContext.Request.HasFormContentType ? httpContext.Request.Form : null;
        var stringBuilder = new StringBuilder();
        if (form != null)
            foreach (var item in form.Keys)
            {
                var keyValue = $"\"{item}\":\"{form[item]}\"";
                stringBuilder.Append($"{keyValue},");
            }

        return stringBuilder.ToString().Replace();
    }

    public static string ReadResponseFormBodyAsString(HttpContext httpContext)
    {
        //var form = httpContext.Response.HasFormContentType ? httpContext.Request.Form : null;
        var stringBuilder = new StringBuilder();
        //if (form != null)
        //{
        //    foreach (var item in form.Keys)
        //    {
        //        string keyValue = $"\"{item}\":\"{form[item]}\"";
        //        stringBuilder.Append($"{keyValue},");
        //    }
        //}
        return stringBuilder.ToString().Replace();
    }


    private static string Replace(this string data)
    {
        return data?.Replace(@"\", "").Replace("\r\n", "").Replace(" ", "").Replace('\\', ' ').Trim();
    }


    //public static void WriteRequestBody(HttpContext context)
    //{
    //Write
    //var jsonString = await ReadRequestBodyExtension.GetRawBodyAsync(context.Request);
    //dynamic jsonObject = JsonConvert.DeserializeObject(jsonString);
    //jsonObject.budgetOrganization = "005555";
    //var modifiedJsonString = JsonConvert.SerializeObject(jsonObject);
    //var array = System.Text.Encoding.UTF8.GetBytes(modifiedJsonString);
    // await context.Request.Body.WriteAsync(array, 0, array.Length);

    //var request = context.Request;

    ////get the request body and put it back for the downstream items to read
    //var stream = request.Body;// currently holds the original stream                    
    //var originalContent = await new StreamReader(stream).ReadToEndAsync();
    //var notModified = true;
    //try
    //{
    //    dynamic dataSource = JsonConvert.DeserializeObject(originalContent);

    //    //replace request stream to downstream handlers
    //    dataSource.budgetOrganization = "005555";
    //    var json = JsonConvert.SerializeObject(dataSource);

    //    var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
    //    stream = await requestContent.ReadAsStreamAsync();//modified stream
    //    var originalContent2 = await new StreamReader(stream).ReadToEndAsync();
    //    notModified = false;

    //}
    //catch
    //{
    //    //No-op or log error
    //}
    //if (notModified)
    //{
    //    //put original data back for the downstream to read
    //    var requestData = Encoding.UTF8.GetBytes(originalContent);
    //    stream = new MemoryStream(requestData);
    //}

    //request.Body = stream;
    // }
}