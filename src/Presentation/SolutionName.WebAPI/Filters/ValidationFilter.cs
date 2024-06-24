using Microsoft.AspNetCore.Mvc.Filters;
using SolutionName.Application.Enums;
using SolutionName.Application.Exceptions;
using SolutionName.Application.Utilities.Extensions;

namespace SolutionName.WebAPI.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
                throw new ApiException(HttpResponseStatus.BindError, null, context.ModelState.AllErrors());
            await next();           
        }

        #region old
        //public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        //{
        //    if (!context.ModelState.IsValid)
        //    {
        //        var errors = context.ModelState
        //               .Where(x => x.Value.Errors.Any())
        //               .ToDictionary(e => e.Key, e => e.Value.Errors.Select(e => e.ErrorMessage))
        //               .ToArray();

        //        context.Result = new BadRequestObjectResult(errors);
        //        return;
        //    }

        //    await next();
        //}
        #endregion

    }
}
