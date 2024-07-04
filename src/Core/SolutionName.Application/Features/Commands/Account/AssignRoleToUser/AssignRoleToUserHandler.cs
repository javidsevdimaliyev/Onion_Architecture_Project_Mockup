using MediatR;
using SolutionName.Application.Abstractions.Services.Authorization;

namespace SolutionName.Application.Features.Commands.AccountCommands.AssignRoleToUser
{
    public class AssignRoleToUserHandler : IRequestHandler<AssignRoleToUserRequest, AssignRoleToUserResponse>
    {
        readonly IUserRoleService _service;
        public AssignRoleToUserHandler(IUserRoleService service)
        {
            _service = service;
        }
        public async Task<AssignRoleToUserResponse> Handle(AssignRoleToUserRequest request, CancellationToken cancellationToken)
        {
            await _service.AssignRoleToUserAsnyc(request.Userid, request.Roles);

            return new() { Success = true };
        }
    }
}
