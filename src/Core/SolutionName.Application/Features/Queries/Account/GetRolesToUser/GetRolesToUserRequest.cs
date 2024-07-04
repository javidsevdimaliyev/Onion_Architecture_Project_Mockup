using MediatR;

namespace SolutionName.Application.Features.Queries.Account.GetRolesToUser
{
    public class GetRolesToUserRequest : IRequest<GetRolesToUserResponse>
    {
        public int UserId { get; set; }
    }
}
