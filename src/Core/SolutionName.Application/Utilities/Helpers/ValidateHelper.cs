using System.ComponentModel.DataAnnotations;
using SolutionName.Application.Enums;
using SolutionName.Application.Utilities.Extensions;

namespace SolutionName.Application.Utilities.Helpers;

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