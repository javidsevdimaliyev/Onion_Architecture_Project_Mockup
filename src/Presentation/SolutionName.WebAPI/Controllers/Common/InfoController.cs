using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SolutionName.Application.Models.Shared;
using SolutionName.Application.Attributes;
using SolutionName.Application.Enums;
using System.Reflection;

namespace SolutionName.WebAPI.Controllers.Common
{
    public class InfoController : Controller
    {
       
        [HttpGet]
        [AuthorizeDefinition(ActionType = ActionType.Reading, RoleName = "Admin")]
        public IActionResult GetAuthorizeDefinitionEndpoints()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var controllers = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ControllerBase)));

            List<AuthDefinitionModel> enpoints = new();
            if (controllers != null)
                foreach (var controller in controllers)
                {
                    var actions = controller.GetMethods().Where(m => m.IsDefined(typeof(AuthorizeDefinitionAttribute)));
                    if (actions != null)
                        foreach (var action in actions)
                        {
                            var attributes = action.GetCustomAttributes(true);
                            if (attributes != null)
                            {
                                AuthDefinitionModel point = null;

                                var authorizeDefinitionAttribute = attributes.FirstOrDefault(a => a.GetType() == typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;
                                //Your logic here...
                            }
                        }
                }


            return Ok(enpoints);
        }
    }
}
