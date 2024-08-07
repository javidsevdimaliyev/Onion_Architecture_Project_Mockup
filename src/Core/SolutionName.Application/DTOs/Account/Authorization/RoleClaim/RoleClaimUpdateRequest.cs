namespace SolutionName.Application.DTOs.Account.Authorization.RoleClaim;

public class RoleClaimUpdateRequest : BaseDto<int>, IMapTo<RoleClaimEntity>
{
    public string RoleIdHash { get; set; }
    [JsonIgnore]
    public int RoleId { get { return Decrypt<int>(RoleIdHash); } set { RoleIdHash = Encrypt(value); } }
    public string ClaimIdHash { get; set; }
    public string ClaimName { get; set; }
    public string ClaimDisplayName { get; set; }
    [JsonIgnore]
    public int ClaimId { get { return Decrypt<int>(ClaimIdHash); } set { ClaimIdHash = Encrypt(value); } }
    public bool IsDeleted { get; set; }
}