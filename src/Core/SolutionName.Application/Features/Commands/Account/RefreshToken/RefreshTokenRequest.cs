using MediatR;
using SolutionName.Application.Features.Commands.Account.Login;

namespace SolutionName.Application.Features.Commands.Account.RefreshToken
{
    public class RefreshTokenRequest : IRequest<LoginByPasswordResponse>
    {
        public string RefreshToken { get; set; }
        public string Token { get; set; }
    }
}
