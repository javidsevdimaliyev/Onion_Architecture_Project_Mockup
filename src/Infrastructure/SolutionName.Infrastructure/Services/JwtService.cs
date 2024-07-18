using System;
using System.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SolutionName.Application.Abstractions.Services;
using SolutionName.Application.Constants;
using SolutionName.Application.Enums;
using SolutionName.Application.Exceptions;
using SolutionName.Application.Utilities.Extensions;


namespace SolutionName.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtTokenConfig _jwtTokenConfig;

        public JwtService(IOptions<JwtTokenConfig> jwtTokenConfig)
        {
            _jwtTokenConfig = jwtTokenConfig.Value;
        }
        public JwtTokenResult GenerateToken(List<System.Security.Claims.Claim> claims)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtTokenConfig.IssuerSigningKey));

            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_jwtTokenConfig.TokenLifeTime);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = accessTokenExpiration,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                Issuer = _jwtTokenConfig.ValidIssuer,
                Audience = _jwtTokenConfig.ValidAudience,
                NotBefore = DateTime.UtcNow,
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);



            return new JwtTokenResult()
            {
                AccessToken = jwtToken,
                RefreshToken = RandomString(25) + Guid.NewGuid(),
                AccessTokenExpiryDate = accessTokenExpiration,
                RefreshTokenExpiryDate = DateTime.UtcNow.AddMinutes(_jwtTokenConfig.RefreshTokenLifeTime)
            };
        }

        public ValidateTokenResult ValidateToken(string token)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = _jwtTokenConfig.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtTokenConfig.IssuerSigningKey)),
                    ValidateIssuer = _jwtTokenConfig.ValidateIssuer,
                    ValidIssuer = _jwtTokenConfig.ValidIssuer,
                    ValidateAudience = _jwtTokenConfig.ValidateAudience,
                    ValidAudience = _jwtTokenConfig.ValidAudience,
                    ValidateLifetime = _jwtTokenConfig.ValidateLifetime,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = false,
                };

                var principal = jwtTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken? validatedToken);

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)
                        throw new ApiException(HttpResponseStatus.InvalidAccessToken);
                    
                    var claim = principal?.Claims?.FirstOrDefault(c => c.Type == CustomClaimTypeConsts.RemoteIpAddress);
                    if (claim != null)
                    {
                        var remoteIpAddress = new HttpContextAccessor().HttpContext?.Connection?.RemoteIpAddress?.ToString();
                        if (claim.Value != remoteIpAddress)
                            throw new UnauthorizedAccessException(HttpResponseStatus.DifferentRemoteIP.GetDescription());
                    }
                }
                else
                    throw new ApiException(HttpResponseStatus.InvalidAccessToken);

                var nameIdentifier = principal.Claims.FirstOrDefault(x => x.Type == CustomClaimTypeConsts.UserId);
                int.TryParse(nameIdentifier.Value, out var userId);

                var exp = principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp);
                var utcExpiryDate = long.Parse(exp.Value);
                var expDate = UnixTimeStampToDateTime(utcExpiryDate);

                return new ValidateTokenResult { IsValid = true, UserId = userId, ExpDate = expDate };
            }
            catch (Exception)
            {
                return new ValidateTokenResult { IsValid = false };
            }
        }

        public bool CanReadToken(string token)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = _jwtTokenConfig.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtTokenConfig.IssuerSigningKey)),
                ValidateIssuer = _jwtTokenConfig.ValidateIssuer,
                ValidIssuer = _jwtTokenConfig.ValidIssuer,
                ValidateAudience = _jwtTokenConfig.ValidateAudience,
                ValidAudience = _jwtTokenConfig.ValidAudience,
                ValidateLifetime = _jwtTokenConfig.ValidateLifetime,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = false,
            };

            var canReadToken = jwtTokenHandler.CanReadToken(token);
            return canReadToken;
        }

        internal string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
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
}
