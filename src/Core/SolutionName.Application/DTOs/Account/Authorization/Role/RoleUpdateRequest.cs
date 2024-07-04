using SolutionName.Application.DTOs.Account.Authorization.RoleClaim;

namespace SolutionName.Application.DTOs.Account.Authorization.Role;

public class RoleUpdateRequest : BaseDto<int>, IMapTo<RoleEntity>
{
    public string Name { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<RoleClaimUpdateRequest> RoleClaims { get; set; }
    public string[] DeletedRoleClaimsHash { get; set; }
    [JsonIgnore]
    public int[] DeletedRoleClaims { get { return Decrypt(DeletedRoleClaimsHash); } }
}