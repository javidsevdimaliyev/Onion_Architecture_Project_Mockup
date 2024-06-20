using MediatR;
using SolutionName.Application.Abstractions.Services.Authentication;

namespace SolutionName.Application.Features.Commands.AccountCommands.PasswordReset
{
    public class UpdatePasswordHandler : IRequestHandler<UpdatePasswordRequest, UpdatePasswordResponse>
    {
        readonly IUserService _userService;

        public UpdatePasswordHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UpdatePasswordResponse> Handle(UpdatePasswordRequest request, CancellationToken cancellationToken)
        {
            await _userService.UpdatePasswordAsync(request);

            return new() { Success = true };
        }
    }
}
