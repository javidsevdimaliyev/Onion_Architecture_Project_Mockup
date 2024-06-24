namespace SolutionName.Application.DTOs.Account.Authorization.Role;

public class RoleTableResponse : BaseDto
{
    public string Name { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsDeleted { get; set; }
    public string IdHash { get; set; }
    [JsonIgnore]
    public int Id { get { return Decrypt<int>(IdHash); } set { IdHash = Encrypt(value); } }

}