using MediatR;

namespace SolutionName.Application.Features.Queries.AccountQueries.GetRolesToUser
{
    public class GetRolesToUserRequest : IRequest<GetRolesToUserResponse>
    {
        public int UserId { get; set; }
    }
}
