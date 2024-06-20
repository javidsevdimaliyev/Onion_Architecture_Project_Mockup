using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace SolutionName.Application.Utilities.Extensions;

public static class ModelStateExtension
{
    public static IEnumerable<ValidationResult> AllErrors(this ModelStateDictionary modelState)
    {
        var result = new List<ValidationResult>();
        var erroneousFields = modelState.Where(ms => ms.Value.Errors.Any())
            .Select(x => new { x.Key, x.Value.Errors });

        foreach (var erroneousField in erroneousFields)
        {
            IEnumerable<string>? fieldKeys = erroneousField.Key?.Split(",");
            var fieldErrors = erroneousField.Errors
                .Select(error => new ValidationResult(error.ErrorMessage, fieldKeys));
            result.AddRange(fieldErrors);
        }

        return result;
    }

    public static string AllErrorsAsString(this ModelStateDictionary modelState)
    {
        var result = "";
        var erroneousFields = modelState.Where(ms => ms.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors });

        foreach (var erroneousField in erroneousFields)
        {
            var fieldKey = erroneousField.Key;
            var fieldErrors = string.Join("\n",
                erroneousField.Errors.Select(error => $"{fieldKey}-{error.ErrorMessage}").ToList());
            result += $"{fieldErrors}\n";
        }

        return result;
    }
}