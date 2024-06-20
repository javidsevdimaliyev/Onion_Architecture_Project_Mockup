using SolutionName.Application.DTOs.Account.Authorization.UserRole;
using SolutionName.Domain.Enums;

namespace SolutionName.Application.DTOs.Account.Authentication.User;

public class UserAddRequest : BaseDto<int>, IMapTo<UserEntity>
{
    #region fields

    private string _name;
    public string Name
    {
        get { return _name; }
        set { _name = value?.Trim().ToUpper(); }
    }

    private string _surname;
    public string Surname
    {
        get { return _surname; }
        set { _surname = value?.Trim().ToUpper(); }
    }

    private string? _username;
    public string? Username
    {
        get => _username;
        set => _username = value?.Trim().Replace(" ", "").ToLower();
    }

    public string Email { get; set; }
    public string? Password { get; set; }

    [JsonIgnore]
    public string? PasswordHash { get; set; }

   
    public AuthenticationTypeEnum AuthenticationType { get; set; }

    public List<UserRoleAddRequest> UserRoles { get; set; } = new List<UserRoleAddRequest>();

    #endregion

    #region methods

    public void AddRoles(params int[] roleIds)
    {
        UserRoles.AddRange(roleIds.Select(x => new UserRoleAddRequest(x)));
    }

    public void AddRoles(params string[] roleIds)
    {
        UserRoles.AddRange(roleIds.Select(x => new UserRoleAddRequest(x)));
    }

    public void AddRoles(params UserRoleAddRequest[] roles)
    {
        UserRoles.AddRange(roles);
    }

  
    #endregion
}