namespace SolutionName.Application.DTOs.Account.Authorization.Claim;

public class ClaimDto : BaseDto<int>, IMapTo<ClaimEntity>
{
    public string Name { get; set; }
    public string Value { get; set; }
    public string Module { get; set; }
    public string Page { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsDeleted { get; set; }
}