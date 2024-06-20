using SolutionName.Domain.Entities.Identity;
using System.Security.Claims;

namespace SolutionName.Application.Abstractions.Services
{
    public interface IJwtTokenService
    {
        public string GenerateJwtToken(UserEntity user, string secret, List<Claim>? claims = null, int expireMin = 30,
            DateTime? date = null);

        public ValidateTokenResult ValidateToken(string token);
        long Validate(string token, string secret);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
