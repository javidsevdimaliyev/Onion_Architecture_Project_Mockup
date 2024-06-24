namespace SolutionName.Application.DTOs.Account.Authorization.Claim;
public class ClaimHierarchDto
{
    public string Module { get; set; }
    public List<ClaimHierarchForPageDto> Pages { get; set; }

}

public class ClaimHierarchForPageDto
{
    public string Page { get; set; }
    public List<ClaimDto> Claims { get; set; }
}
