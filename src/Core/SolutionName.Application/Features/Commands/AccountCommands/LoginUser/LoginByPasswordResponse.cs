using System.Text.Json.Serialization;

namespace SolutionName.Application.Features.Commands.AccountCommands.Login
{
    public record LoginByPasswordResponse(string Token, string RefreshToken);
    
}
