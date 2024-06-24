using System.Runtime.Serialization;
using SolutionName.Application.Exceptions;

namespace SolutionName.Application.Models.Common;

[DataContract]
public class APIResponse
{
    public APIResponse()
    {
        ServerName = Environment.MachineName;
    }

    public APIResponse(int statusCode, string message = "", object? result = null, ApiException? apiError = null,
        string apiVersion = "1.0.0.0", string executingTime = "")
    {
        StatusCode = statusCode;
        Message = message;
        Result = result;
        ResponseException = apiError;
        Version = apiVersion;
        ExecutingTime = executingTime;
        IsSuccess = apiError == null;
        ServerName = Environment.MachineName;
    }

    [DataMember] public string Version { get; set; }

    [DataMember] public bool IsSuccess { get; private set; }

    [DataMember] public int StatusCode { get; set; }

    [DataMember(EmitDefaultValue = false)] public string Message { get; set; }

    [DataMember(EmitDefaultValue = false)] public ApiException ResponseException { get; set; }

    [DataMember(EmitDefaultValue = false)] public object? Result { get; set; }

    [DataMember(EmitDefaultValue = false)] public string ExecutingTime { get; set; }

    [DataMember] public string ServerName { get; set; }
}