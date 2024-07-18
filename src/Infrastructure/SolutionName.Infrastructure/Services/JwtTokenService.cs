using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SolutionName.Application.Abstractions.Services;
using SolutionName.Application.Constants;
using SolutionName.Application.Enums;
using SolutionName.Application.Utilities.Extensions;
using SolutionName.Domain.Entities.Identity;


namespace SolutionName.Infrastructure.Services;

public class JwtTokenService : ISecurityTokenValidator, IJwtTokenService
{
    private readonly AudienceConfig _settings;
    private readonly System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler _tokenHandler;
    public JwtTokenService(IOptions<AudienceConfig> settings)
    {
        _settings = settings.Value;
        _tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
    }

    public bool CanValidateToken { get; } = true;
    public int MaximumTokenSizeInBytes { get; set; } = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;

    public bool CanReadToken(string securityToken)
    {
        //if ()
        //    return true;

        return _tokenHandler.CanReadToken(securityToken);
    }

    public string GenerateJwtToken(UserEntity user, string secret, List<Claim>? claims = null,
        int expireMin = 30, DateTime? date = null)
    {
        date ??= DateTime.Now;
        var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        claims ??= new List<Claim>();
        claims.Add(new Claim("id", user.Id.ToString()));
        if (claims.Any(x => x.Type == CustomClaimTypeConsts.UserId) == false)
            claims.Add(new Claim(CustomClaimTypeConsts.UserId, user.Id.ToString()));
        var jwt = new JwtSecurityToken(
            _settings.Iss,
            _settings.Aud,
            claims,
            date,
            date.Value.AddMinutes(expireMin),
            new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );
        var token = _tokenHandler.WriteToken(jwt);
        return token;
    }

    public long Validate(string token, string secret)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ClockSkew = TimeSpan.Zero
        };


        try
        {
            _tokenHandler.ValidateToken(token, validationParameters,
                out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = long.Parse(jwtToken.Claims.First(x => x.Type == CustomClaimTypeConsts.UserId).Value);
            return userId;
        }
        catch (Exception exception)
        {
            return 0;
        }
    }

    public ValidateTokenResult ValidateToken(string token)
    {
        if (token == null)
            return new ValidateTokenResult { IsValid = false };
     
        try
        {
            var principal = ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);
           
            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == CustomClaimTypeConsts.UserId).Value);
            var exp = principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp);
            var utcExpiryDate = long.Parse(exp.Value);
            var expDate = UnixTimeStampToDateTime(utcExpiryDate);

            return new ValidateTokenResult { IsValid = true, UserId = userId, ExpDate = expDate };
        }
        catch (Exception exception)
        {
            // return null if validation fails
            //return null;
            throw new UnauthorizedAccessException(HttpResponseStatus.Unauthorized.GetDescription());
        }
    }

    public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters,
    out SecurityToken validatedToken)
    {
        try
        {
            var principal = _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);          
            var claim = principal?.Claims?.FirstOrDefault(c => c.Type == CustomClaimTypeConsts.RemoteIpAddress);
            if (claim != null)
            {
                var remoteIpAddress = new HttpContextAccessor().HttpContext?.Connection?.RemoteIpAddress?.ToString();
                if (claim.Value != remoteIpAddress)
                    throw new UnauthorizedAccessException(HttpResponseStatus.DifferentRemoteIP.GetDescription());
            }

            return principal;
        }
        catch (Exception e)
        {
            throw new UnauthorizedAccessException();
        }
    }

  

   
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var key = Encoding.ASCII.GetBytes(_settings.Secret);
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
        };

        SecurityToken securityToken;
        var principal = _tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");
        return principal;
    }



    private async Task<string> GetUniqueToken()
    {
        // token is a cryptographically strong random sequence of values
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        return token;
    }

    internal DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new(1970,
                                1,
                                1,
                                0,
                                0,
                                0,
                                0,
                                DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }
    
}

