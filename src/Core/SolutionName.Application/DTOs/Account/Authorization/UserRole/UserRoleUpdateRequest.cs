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
    public string IdHash { get; set; }
    [JsonIgnore]
    public int Id { get { return Decrypt<int>(IdHash); } set { IdHash = Encrypt(value); } }

    public string RoleName { get; set; }

}