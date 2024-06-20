using MediatR;
using SolutionName.Application.Abstractions.Services.Authentication;

namespace SolutionName.Application.Features.Commands.AccountCommands.Login
{
    public class LoginByPasswordHandler : IRequestHandler<LoginByPasswordRequest, LoginByPasswordResponse>
    {
        readonly IAuthService _authService;
        public LoginByPasswordHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public Task<LoginByPasswordResponse> Handle(LoginByPasswordRequest request, CancellationToken cancellationToken)
        {
            return _authService.LoginAsync(request, cancellationToken);
        }
    }
}
