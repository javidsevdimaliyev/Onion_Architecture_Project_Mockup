using SolutionName.Application.Abstractions.Services.Authentication;
using SolutionName.Application.Features.Commands.AccountCommands.Login;

namespace SolutionName.Application.Features.Commands.AccountCommands.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, LoginByPasswordResponse>
    {
        readonly IAuthService _authService;

        public RefreshTokenHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginByPasswordResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            return await _authService.RefreshTokenAsync(request, cancellationToken);
        }
    }
}
