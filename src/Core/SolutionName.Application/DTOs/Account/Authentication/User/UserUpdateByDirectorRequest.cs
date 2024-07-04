using SolutionName.Application.DTOs.Account.Authorization.UserRole;

namespace SolutionName.Application.DTOs.Account.Authentication.User;

public class UserUpdateByDirectorRequest : BaseDto<int>, IMapTo<UserEntity>
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

    private string _fathername;
    public string Fathername
    {
        get { return _fathername; }
        set { _fathername = value?.Trim().ToUpper(); }
    }

    public string? Password { get; set; }

    private string _pin;
    public string Pin
    {
        get { return _pin; }
        set { _pin = value?.Trim(); }
    }

    public List<UserRoleUpdateRequest> UserRoles { get; set; } = new List<UserRoleUpdateRequest>();

    public string[] DeletedUserRolesHash { get; set; }
    [JsonIgnore]
    public int[] DeletedUserRoles { get { return Decrypt(DeletedUserRolesHash); } }

    public string[] DeletedUserPositionsHash { get; set; }
    [JsonIgnore]
    public int[] DeletedUserPositions { get { return Decrypt(DeletedUserPositionsHash); } }
    #endregion
}