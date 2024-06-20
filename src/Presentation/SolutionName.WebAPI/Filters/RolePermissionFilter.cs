using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using SolutionName.Application.Abstractions.Services.Authorization;
using SolutionName.Application.Attributes;
using System.Reflection;

namespace SolutionName.WebAPI.Filters
{
    public class RolePermissionFilter : IAsyncActionFilter
    {
        readonly IUserRoleService _service;

        public RolePermissionFilter(IUserRoleService service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var name = context.HttpContext.User.Identity?.Name;
            if (!string.IsNullOrEmpty(name))
            {
                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
                var attribute = descriptor.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;
                //var controllerName = context.ActionDescriptor.RouteValues["controller"];
                //var actionName = context.ActionDescriptor.RouteValues["action"];
                //var httpAttribute = descriptor.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute)) as HttpMethodAttribute;
                //var action = $"{attribute.RoleName.Replace(" ", "")}.{controllerName}.{actionName}.{attribute.ActionType}.";

                var hasRole = await _service.HasRolePermissionToEndpointAsync(attribute.RoleName);

                if (!hasRole)
                    context.Result = new UnauthorizedResult();
                else
                    await next();
            }
            else
                await next();
        }
    }
}
