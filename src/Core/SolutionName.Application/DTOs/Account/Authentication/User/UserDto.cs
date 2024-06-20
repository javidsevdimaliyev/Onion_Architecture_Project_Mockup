using SolutionName.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SolutionName.Application.DTOs.Account.Authentication.User
{
    public class UserDto : BaseDto<int>, IMapTo<UserEntity>
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
