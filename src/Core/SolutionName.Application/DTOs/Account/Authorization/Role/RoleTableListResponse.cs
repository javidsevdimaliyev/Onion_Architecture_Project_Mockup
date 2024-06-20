namespace SolutionName.Application.DTOs.Account.Authorization.Role;

public class RoleTableListResponse : BaseDto
{
    public int Count { get; set; }
    public ICollection<RoleTableResponse> Data { get; set; }

}