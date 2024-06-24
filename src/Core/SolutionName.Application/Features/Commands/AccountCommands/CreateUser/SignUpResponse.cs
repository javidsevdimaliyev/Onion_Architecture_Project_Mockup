using SolutionName.Application.Models.Responses;
using SolutionName.Application.Exceptions;
using SolutionName.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SolutionName.Application.Features.Commands.AccountCommands.Create
{
    public class SignUpResponse : IMapTo<UserEntity>
    {
        public string? Name { get; set; }

        public string? Surname { get; set; }
        public string PasswordHash { get; set; }

        public string? RefreshToken { get; set; }

        public AuthenticationTypeEnum? AuthenticationType { get; set; }

        [Timestamp]
        public byte[] TimeStamp { get; set; }
    }
}
