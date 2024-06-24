using AutoMapper;
using Microsoft.AspNetCore.Http;
using SolutionName.Application.Abstractions.Services;
using SolutionName.Application.Abstractions.Services.Authentication;
using SolutionName.Application.Abstractions.Services.Authorization;
using SolutionName.Application.Constants;
using SolutionName.Application.DTOs.Account.Authentication.User;
using SolutionName.Application.Features.Commands.AccountCommands.Login;
using SolutionName.Application.Features.Commands.AccountCommands.RefreshToken;
using SolutionName.Application.Utilities.Extensions;
using SolutionName.Domain.Enums;
using System.Security.Claims;

namespace SolutionName.Application.Services.Authentication
{
    public class AuthService : IAuthService
    {
        #region ..:fields:..

        readonly IMapper _mapper;
        readonly IRoleService _roleService;
        readonly IUserService _userService;
        readonly IJwtService _jwtService;
        readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region ..:ctors:..

        public AuthService(IJwtService jwtService,

            IMapper mapper
,
            IRoleService roleService,
            IUserService userService,
            IHttpContextAccessor httpContextAccessor)
        {
            _jwtService = jwtService;
            _mapper = mapper;
            _roleService = roleService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region ..:custom login methods:..

        public async Task<LoginByPasswordResponse> LoginAsync(LoginByPasswordRequest model, CancellationToken cancellationToken)
        {
            UserDto currentUser = await _userService.GetUserByAsync(
                x => x.Username == model.UsernameOrEmail,
                catchException: true,
                cancellationToken);

            // if (model.Password != "admin")
            await _userService.VerifyPasswordAsync(model.Password, currentUser, cancellationToken);

            return await HandleLoginResponseAsync(
                currentUser: currentUser,
                authenticateType: AuthenticationTypeEnum.UsernamePassword,
                cancellationToken: cancellationToken);
        }

        public async Task<LoginByPasswordResponse> RefreshTokenAsync(RefreshTokenRequest model, CancellationToken cancellationToken)
        {
            var validateToken = _jwtService.ValidateToken(model.Token);
            if (!validateToken.IsValid)
                throw new UnauthorizedAccessException(HttpResponseStatus.ExpiredRefreshToken.GetDescription());

            await _userService.ValidateRefreshTokenAsync(validateToken.UserId, model.RefreshToken, cancellationToken);

            var roles = await _roleService.GetRolesByUserIdAsync(validateToken.UserId, cancellationToken);
           
            var claims = new List<System.Security.Claims.Claim>()
            {
                new System.Security.Claims.Claim(CustomClaimTypeConsts.AuthenticationType, ((int)AuthenticationTypeEnum.UsernamePassword).ToString()),
                new System.Security.Claims.Claim(CustomClaimTypeConsts.UserId, validateToken.UserId.ToString()),
                new System.Security.Claims.Claim(CustomClaimTypeConsts.Roles, string.Join(',', roles.Select(x => x.Id))),
                //new System.Security.Claims. Claim(CustomClaimTypes.SchemaIds, string.Join(',',roles.Select(x=>x.SchemaId).Distinct())),
            };

            var result = await _userService.UpdateRefreshTokenAsync(validateToken.UserId, claims, cancellationToken);

            return new LoginByPasswordResponse(result.AccessToken, result.RefreshToken);
        }



        #endregion

        #region private methods
        private async Task<LoginByPasswordResponse> HandleLoginResponseAsync(
            UserDto currentUser,
            AuthenticationTypeEnum authenticateType,
            List<System.Security.Claims.Claim>? additionalClaims = null,
            CancellationToken cancellationToken = default)
        {
            ThrowExceptionExtension.ThrowIfTrue(currentUser is null, HttpResponseStatus.DomainUserNotFound);

            var rolesWithSchemas = await _roleService.GetRolesByUserIdAsync(currentUser!.Id, cancellationToken);
            var roleIds = rolesWithSchemas.Select(x => x.Id).ToArray();

            // Get claims of user
            var roleClaims = await _userService.GetRoleClaimsByRoleId(roleIds, cancellationToken);
            var remoteIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var claims = new List<System.Security.Claims.Claim>()
            {
                new System.Security.Claims.Claim(CustomClaimTypeConsts.AuthenticationType, ((int)authenticateType).ToString()),
                new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, currentUser.Id.ToString()),
                new System.Security.Claims.Claim(CustomClaimTypeConsts.UserId, currentUser.Id.ToString()),
                new System.Security.Claims.Claim(CustomClaimTypeConsts.Roles, string.Join(',', roleIds)),
                new System.Security.Claims.Claim(CustomClaimTypeConsts.RemoteIpAddress, remoteIpAddress)
            };

            if (additionalClaims is not null && additionalClaims.Any())
                claims.AddRange(additionalClaims);

            if (roleClaims is not null && roleClaims.Any())
                claims.AddRange(roleClaims);

            var result = await _userService.UpdateRefreshTokenAsync(
                userId: currentUser.Id,
                claims: claims,
                cancellationToken: cancellationToken);

            if (currentUser.AuthenticationType is null)
            {
                await _userService.EditAuthTypeAsync(
                        id: currentUser.Id,
                        authType: authenticateType,
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }

            return new LoginByPasswordResponse(result.AccessToken, result.RefreshToken);
        }


        #endregion
    }
}
