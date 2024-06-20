using System.ComponentModel.DataAnnotations;
using SolutionName.Application.Shared.Enums;

namespace SolutionName.Application.Shared.Utilities.Helpers;

public static class ValidateHelper
{
    public static void Add(this List<ValidationResult> result,
        HttpResponseStatus messageEnum, params string[] propertyName)
    {
        result.Add(new ValidationResult(messageEnum.GetDescription(), propertyName));
    }

    public static void Add(this List<ValidationResult> result,
        string error, params string[] propertyName)
    {
        result.Add(new ValidationResult(error, propertyName));
    }
}