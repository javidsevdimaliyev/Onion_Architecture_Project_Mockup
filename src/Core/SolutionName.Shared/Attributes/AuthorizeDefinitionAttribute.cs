using SolutionName.Application.Shared.Enums;

namespace SolutionName.Application.Shared.Attributes
{
    public class AuthorizeDefinitionAttribute : Attribute
    {      
        public string RoleName { get; set; }
        public ActionType ActionType { get; set; }
    }
}
