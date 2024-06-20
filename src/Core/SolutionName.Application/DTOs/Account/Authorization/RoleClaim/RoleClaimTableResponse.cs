namespace SolutionName.Application.DTOs.Account.Authorization.RoleClaim;

public class RoleClaimTableResponse : BaseDto
{
    public string RoleIdHash { get; set; }
    [JsonIgnore]
    public int RoleId { get { return Decrypt<int>(RoleIdHash); } set { RoleIdHash = Encrypt(value); } }
    public string ClaimIdHash { get; set; }
    [JsonIgnore]
    public int ClaimId { get { return Decrypt<int>(ClaimIdHash); } set { ClaimIdHash = Encrypt(value); } }
    public bool IsDeleted { get; set; }
    public string RoleName { get; set; }
    public string ClaimName { get; set; }
    public string IdHash { get; set; }
    [JsonIgnore]
    public int Id { get { return Decrypt<int>(IdHash); } set { IdHash = Encrypt(value); } }

}