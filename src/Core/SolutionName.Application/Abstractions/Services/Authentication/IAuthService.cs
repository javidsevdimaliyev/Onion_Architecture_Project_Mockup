using Microsoft.AspNetCore.Identity.Data;
using SolutionName.Application.Features.Commands.Account.Login;
using SolutionName.Application.Features.Commands.Account.RefreshToken;

namespace SolutionName.Application.Abstractions.Services.Authentication
{
    public interface IAuthService
    {
        Task<LoginByPasswordResponse> LoginAsync(LoginByPasswordRequest model, CancellationToken cancellationToken);
        Task<LoginByPasswordResponse> RefreshTokenAsync(RefreshTokenRequest model, CancellationToken cancellationToken);
    }
}
