using SolutionName.Application.DTOs.Account.Authentication.User;
using SolutionName.Application.Features.Commands.AccountCommands.Create;
using SolutionName.Application.Features.Commands.AccountCommands.PasswordReset;
using SolutionName.Application.Models.Requests;
using SolutionName.Domain.Enums;
using System.Linq.Expressions;

namespace SolutionName.Application.Abstractions.Services.Authentication
{
    public interface IUserService 
    {
        Task<UserDto> GetUserByAsync(
            Expression<Func<UserEntity, bool>> match, bool catchException,
            CancellationToken cancellationToken = default);

        Task<UserTableListResponse> GetTableAsync(
            PagingRequest pagingRequest,
            CancellationToken cancellationToken = default);

        Task<UserUpdateRequest> GetForUpdateByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<SignUpResponse> AddAsync(
            SignUpRequest model,
            CancellationToken cancellationToken = default);

        Task EditAsync(
            UserUpdateRequest model,
            CancellationToken cancellationToken = default);

        Task DeleteByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task EditAuthTypeAsync(
            int id,
            AuthenticationTypeEnum authType,
            CancellationToken cancellationToken = default);

        Task<UserDto> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<List<System.Security.Claims.Claim>?> GetRoleClaimsByUserId(
            int userId,
            CancellationToken cancellationToken = default);

        Task<List<System.Security.Claims.Claim>?> GetRoleClaimsByRoleId(
            int[] roleIds,
            CancellationToken cancellationToken = default);

        Task VerifyPasswordAsync(
            string password,
            UserDto currentUser,
            CancellationToken cancellationToken = default);

        Task UpdatePasswordAsync(UpdatePasswordRequest request);

        ValueTask<bool> ValidateRefreshTokenAsync(
            int userId, 
            string refreshToken, 
            CancellationToken cancellationToken = default);

        Task<JwtTokenResult> UpdateRefreshTokenAsync(
            int userId,
            List<System.Security.Claims.Claim> claims,
            CancellationToken cancellationToken = default);
    }
}
