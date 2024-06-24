using SolutionName.Application.DTOs.Account.Authorization.RoleClaim;

namespace SolutionName.Application.DTOs.Account.Authorization.Role;

public class RoleAddRequest : BaseDto<int>, IMapTo<RoleEntity>
{
    public string Name { get; set; }
    public int DisplayOrder { get; set; }
    public ICollection<RoleClaimAddRequest> RoleClaims { get; set; }

}