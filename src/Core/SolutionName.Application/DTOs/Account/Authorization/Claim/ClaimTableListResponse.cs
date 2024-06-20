namespace SolutionName.Application.DTOs.Account.Authorization.Claim;

public class ClaimTableListResponse : BaseDto
{
    public int Count { get; set; }
    public ICollection<ClaimTableResponse> Data { get; set; }

}