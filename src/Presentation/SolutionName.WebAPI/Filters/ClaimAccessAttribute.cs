using Microsoft.AspNetCore.Mvc.Filters;
using SolutionName.Application.Enums;
using SolutionName.Application.Exceptions;
using SolutionName.Application.Utilities.Helpers;

namespace SolutionName.Application.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
public class ClaimAccessAttribute : ActionFilterAttribute
{
    public string Claim { get; set; }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var controllerName = filterContext.ActionDescriptor.RouteValues["controller"];
        var actionName = filterContext.ActionDescriptor.RouteValues["action"];

        if (string.IsNullOrEmpty(Claim)) Claim = $"{controllerName}_{actionName}";
        if (!IdentityHelper.HasClaim(Claim)) throw new ApiException(HttpResponseStatus.PermissionDenied);
    }
}