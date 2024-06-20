using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionName.Application.Abstractions.Services
{
    public interface IJwtService
    {
        JwtTokenResult GenerateToken(List<System.Security.Claims.Claim> claims);
        ValidateTokenResult ValidateToken(string token);
        bool CanReadToken(string token);
    }

    public class JwtTokenResult
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime AccessTokenExpiryDate { get; set; }
        public DateTime RefreshTokenExpiryDate { get; set; }
    }

    public class ValidateTokenResult
    {
        public bool IsValid { get; set; }
        public int UserId { get; set; }
        public DateTime ExpDate { get; set; }
    }
}
