namespace SolutionName.Application.DTOs.Account.Authentication.User;

public class UserTableResponse : BaseDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Fathername { get; set; }
    public string Username { get; set; }
    public string? CompanyName { get; set; }
    public string PasswordHash { get; set; }
    public string Tin { get; set; }
    public string Pin { get; set; }
    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; }
    public string IdHash { get; set; }
    [JsonIgnore]
    public int Id { get { return Decrypt<int>(IdHash); } set { IdHash = Encrypt(value); } }

    public bool IsView { get; set; }
}