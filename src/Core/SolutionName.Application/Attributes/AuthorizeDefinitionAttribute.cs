using SolutionName.Application.Enums;

namespace SolutionName.Application.Attributes
{
    public class AuthorizeDefinitionAttribute : Attribute
    {      
        public string RoleName { get; set; }
        public ActionType ActionType { get; set; }
    }
}
