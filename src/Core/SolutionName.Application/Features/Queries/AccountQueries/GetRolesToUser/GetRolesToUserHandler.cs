using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionName.Application.Features.Queries.AccountQueries.GetRolesToUser
{
    public class GetRolesToUserHandler : IRequestHandler<GetRolesToUserRequest, GetRolesToUserResponse>
    {
        public Task<GetRolesToUserResponse> Handle(GetRolesToUserRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
