using System.Text.Json.Serialization;

namespace SolutionName.Application.Features.Commands.Account.Login
{
    public record LoginByPasswordResponse(string Token, string RefreshToken);
    
}
