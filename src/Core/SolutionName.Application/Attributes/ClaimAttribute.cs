namespace SolutionName.Application.Attributes;

public class ClaimAttribute : Attribute
{
    public ClaimAttribute(string module, string page)
    {
        this.Module = module;
        this.Page = page;
    }

    public string Module { get; set; }
    public string Page { get; set; }
}