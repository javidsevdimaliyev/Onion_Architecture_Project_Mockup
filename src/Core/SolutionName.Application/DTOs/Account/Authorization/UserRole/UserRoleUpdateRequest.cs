namespace SolutionName.Application.DTOs.Account.Authorization.UserRole;

public class UserRoleUpdateRequest : BaseDto<int>, IMapTo<UserRoleEntity>
{
    public string RoleIdHash { get; set; }
    [JsonIgnore]
    public int RoleId { get { return Decrypt<int>(RoleIdHash); } set { RoleIdHash = Encrypt(value); } }
    public string UserIdHash { get; set; }
    [JsonIgnore]
    public int UserId { get { return Decrypt<int>(UserIdHash); } set { UserIdHash = Encrypt(value); } }
    public bool IsDeleted { get; set; }
    public string RoleName { get; set; }

}