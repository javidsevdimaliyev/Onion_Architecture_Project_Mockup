namespace SolutionName.Application.DTOs.Account.Authentication.User;

public class UserTableListResponse : BaseDto
{
    public int Count { get; set; }
    public ICollection<UserTableResponse> Data { get; set; }

}