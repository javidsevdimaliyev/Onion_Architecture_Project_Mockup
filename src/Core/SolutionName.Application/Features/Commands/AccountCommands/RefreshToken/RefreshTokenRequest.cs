using MediatR;
using SolutionName.Application.Features.Commands.AccountCommands.Login;

namespace SolutionName.Application.Features.Commands.AccountCommands.RefreshToken
{
    public class RefreshTokenRequest : IRequest<LoginByPasswordResponse>
    {
        public string RefreshToken { get; set; }
        public string Token { get; set; }
    }
}
