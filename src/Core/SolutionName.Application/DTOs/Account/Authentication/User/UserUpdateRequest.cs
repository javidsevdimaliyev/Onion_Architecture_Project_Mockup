using SolutionName.Application.DTOs.Account.Authorization.UserRole;

namespace SolutionName.Application.DTOs.Account.Authentication.User;

public class UserUpdateRequest : BaseDto<int>, IMapTo<UserEntity>
{
    public string IdHash { get; set; }
    [JsonIgnore]
    public int Id { get { return Decrypt<int>(IdHash); } set { IdHash = Encrypt(value); } }

    public string Name { get; set; }
    public string Surname { get; set; }
    public string Fathername { get; set; }
    public string? Username { get; set; }
    public string CompanyName { get; set; }
    public string? Password { get; set; }
    public string Tin { get; set; }
    public string Pin { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<UserRoleUpdateRequest> UserRoles { get; set; } = new List<UserRoleUpdateRequest>();
    public string[] DeletedUserRolesHash { get; set; }
    [JsonIgnore]
    public int[] DeletedUserRoles { get { return Decrypt(DeletedUserRolesHash); } }

}