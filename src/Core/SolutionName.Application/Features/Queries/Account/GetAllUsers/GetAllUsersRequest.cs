using MediatR;
using SolutionName.Application.Models.Requests;

namespace SolutionName.Application.Features.Queries.Account.GetAllUsers
{
    public class GetAllUsersRequest : IRequest<GetAllUsersResponse>
    {
        public PagingRequest PagingRequest { get; set; }
    }
}
