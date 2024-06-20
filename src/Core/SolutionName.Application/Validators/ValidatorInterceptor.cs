using Application.Shared.Resources;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SolutionName.Application.Enums;
using System.Text.Json;

namespace SolutionName.Application.Validators
{
    public class ValidatorInterceptor : IValidatorInterceptor
    {
        public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext) =>
            commonContext;

        public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext,
            ValidationResult result)
        {
            List<ValidationFailure> failures = new();
            ValidationFailure validationFailure;
            foreach (var failure in result.Errors)
            {
                var validationError = new ValidationError
                {
                    FieldName = failure.PropertyName,
                    Pattern = failure.CustomState?.ToString()
                };

                if (Enum.TryParse(failure.ErrorCode, out ValidationType validationTypeResult))
                {
                    validationError.ValidationType = failure.ErrorCode;
                    validationError.Message = (validationTypeResult is ValidationType.RequiredDefault
                                               || validationTypeResult is ValidationType.NotEmptyValidator
                                               || validationTypeResult is ValidationType.NotNullValidator)
                        ? SharedResources.RequiredAttribute
                        : failure.ErrorMessage;
                }
                else
                {
                    if (failure.CustomState is not null)
                    {
                        validationError.ValidationType = nameof(ValidationType.Pattern);
                        validationError.Message = failure.ErrorMessage;
                    }
                    else
                    {
                        validationError.ValidationType = nameof(ValidationType.RequiredDefault);
                        validationError.Message = failure.ErrorCode is not null
                            ? failure.ErrorMessage
                            : SharedResources.RequiredAttribute;
                    }
                }

                validationFailure = new ValidationFailure(failure.PropertyName, JsonSerializer.Serialize(validationError));
                failures.Add(validationFailure);
            }

            return new ValidationResult(failures);

            //var projection = result.Errors.Select(
            //    failure => new ValidationFailure(
            //        failure.PropertyName,
            //        JsonSerializer.Serialize(new ValidationError
            //        {
            //            FieldName = failure.PropertyName,
            //            Message = Enum.TryParse(failure.ErrorCode, out ValidationType validationType) ? validationType is ValidationType.Required ? SharedResources.RequiredAttribute : failure.ErrorMessage : failure.ErrorMessage,
            //            ValidationType = Enum.TryParse(failure.ErrorCode, out ValidationType result) ? failure.ErrorCode : failure.CustomState is not null ? nameof(ValidationType.Pattern) : nameof(ValidationType.Required),
            //            Pattern = failure.CustomState?.ToString()
            //        })));


            //return new ValidationResult(projection);
        }

        public class ValidationError
        {
            public string FieldName { get; set; }
            public string ValidationType { get; set; }
            public string? Pattern { get; set; }
            public string Message { get; set; }
        }
    }
}
