namespace SolutionName.Application.DTOs.Account.Authorization.Claim;

public class ClaimUpdateRequest : BaseDto<int>, IMapTo<ClaimEntity>
{
    public string Name { get; set; }
    public string Module { get; set; }
    public string Page { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsDeleted { get; set; }
    public string IdHash { get; set; }
}