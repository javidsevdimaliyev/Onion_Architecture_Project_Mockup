namespace SolutionName.Application.DTOs.Account.Authorization.UserRole;


public class UserRoleAddRequest : BaseDto<int>, IMapTo<UserRoleEntity>
{
    public UserRoleAddRequest()
    {

    }

    public UserRoleAddRequest(int roleId) : this()
    {
        RoleId = roleId;
    }

    public UserRoleAddRequest(string roleIdHash) : this()
    {
        RoleIdHash = roleIdHash;
    }

    public string RoleIdHash { get; set; }
    [JsonIgnore]
    public int RoleId { get { return Decrypt<int>(RoleIdHash); } set { RoleIdHash = Encrypt(value); } }
    public string UserIdHash { get; set; }
    [JsonIgnore]
    public int UserId { get { return Decrypt<int>(UserIdHash); } set { UserIdHash = Encrypt(value); } }

}