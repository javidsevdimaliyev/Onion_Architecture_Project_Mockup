using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SolutionName.Application.Shared.Utilities.Helpers;

public class DependencyHelper
{
    public static T GetService<T>()
    {
        return new HttpContextAccessor().HttpContext.RequestServices.GetService<T>();
    }
}