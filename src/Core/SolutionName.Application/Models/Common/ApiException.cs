using SolutionName.Application.Utilities.Extensions;
using SolutionName.Application.Enums;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace SolutionName.Application.Exceptions;

public class ApiException : Exception
{
    public ApiException(HttpResponseStatus status = HttpResponseStatus.Exception,
        string message = "",
        IEnumerable<ValidationResult> errors = null) :
        base(string.IsNullOrWhiteSpace(message) ? status.GetDescription() : message)
    {
        Status = status;
        ValidationErrors = errors;
    }

    public ApiException(string message,
        HttpResponseStatus status = HttpResponseStatus.Exception,
        IEnumerable<ValidationResult> errors = null,
        string errorCode = "",
        string refLink = "") :
        base(message)
    {
        Status = status;
        ValidationErrors = errors;
        ReferenceErrorCode = errorCode;
        ReferenceDocumentLink = refLink;
    }

    public ApiException(Exception ex, HttpResponseStatus status = HttpResponseStatus.Exception)
        : base(ex.Message)
    {
        Status = status;
    }

    public ApiException(HttpResponseStatus status) : base(status.GetDescription())
    {
        Status = status;
        StatusCode = (int)status;
    }

    public int? StatusCode { get; set; }

    [DataMember]
    public string? Details { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public string? ReferenceErrorCode { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public string? ReferenceDocumentLink { get; set; }

    [JsonIgnore]
    public override string? StackTrace => base.StackTrace;

    [JsonIgnore]
    public override string? Source
    {
        get => base.Source;
        set => base.Source = value;
    }

    public override IDictionary Data => base.Data;


    [JsonIgnore] public HttpResponseStatus Status { get; set; }


    public IEnumerable<ValidationResult> ValidationErrors { get; set; }
}