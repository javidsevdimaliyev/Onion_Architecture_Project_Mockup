using Application.Shared.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SolutionName.Application.Abstractions.Repositories.EntityFramework;
using SolutionName.Application.Abstractions.Services;
using SolutionName.Application.Abstractions.Services.Authentication;
using SolutionName.Application.Constants;
using SolutionName.Application.DTOs.Account.Authentication.User;
using SolutionName.Application.Features.Commands.Account.Create;
using SolutionName.Application.Features.Commands.Account.UpdatePassword;
using SolutionName.Application.Models.Requests;
using SolutionName.Application.Utilities.Extensions;
using SolutionName.Domain.Enums;
using System.Linq.Expressions;

namespace SolutionName.Application.Concretes.Authentication
{
    public class UserService : IUserService
    {
        #region fields

        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IJwtService _jwtService;
        readonly UserManager<UserEntity> _userManager;
        readonly IRepository<UserEntity, int> _userRepository;
        readonly IRepository<UserRoleEntity, int> _userRoleRepository;
        readonly IRepository<RoleClaimEntity, int> _roleClaimRepository;

        #endregion

        #region ctors

        public UserService(
            IMapper mapper,
            UserManager<UserEntity> userManager,
            IUnitOfWork unitOfWork,
            IJwtService jwtService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _userRepository = unitOfWork.Repository<UserEntity, int>();
            _userRoleRepository = unitOfWork.Repository<UserRoleEntity, int>();
            _roleClaimRepository = unitOfWork.Repository<RoleClaimEntity, int>();
            _jwtService = jwtService;
        }

        #endregion

        #region methods

        #region queries

        public async Task<UserDto> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            // var entity = await GetAsync(id, cancellationToken);
            var entity = await _userManager.FindByIdAsync(id.ToString());

            return _mapper.Map<UserDto>(entity);
        }

        public async Task<UserDto?> GetUserByAsync(Expression<Func<UserEntity, bool>> match, bool catchException, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(match);

            if (catchException == false && user is null)
                return null;

            ThrowExceptionExtension.ThrowIfNull(user, HttpResponseStatus.UserNotFound);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserTableListResponse> GetTableAsync(PagingRequest pagingRequest, CancellationToken cancellationToken)
        {
            var query = from o in _userManager.Users
                        select new UserTableResponse
                        {
                            Name = o.Name,
                            Surname = o.Surname,
                            Username = o.Username,
                            IsDeleted = o.IsDeleted,
                            Id = o.Id,
                            CreatedDate = o.CreatedDate
                        };

            query = query.AddFilter(pagingRequest, out int count);

            return new UserTableListResponse
            {
                Count = count,
                Data = await query.ToListAsync(cancellationToken: cancellationToken)
            };
        }

        public async Task<UserUpdateRequest> GetForUpdateByIdAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await _userManager.Users.Where(x => x.Id == id)
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            entity.ThrowIfNull(HttpResponseStatus.UserNotFound);

            var model = _mapper.Map<UserUpdateRequest>(entity);

            foreach (var role in model.UserRoles)
                role.RoleName = entity.UserRoles.FirstOrDefault(x => x.RoleId == role.RoleId)?.Role.Name;

            return model;

        }

        public async Task<List<System.Security.Claims.Claim>?> GetRoleClaimsByUserId(int userId, CancellationToken cancellationToken)
        {
            // Get user role IDs
            var userRoleIds = await _userRoleRepository
                .Query().Where(x => x.UserId == userId)
                .Select(x => x.RoleId).ToArrayAsync(cancellationToken).ConfigureAwait(false);

            // Get role claims for the user
            var userClaims = await _roleClaimRepository.FindQueryable()
                .Include(x => x.Claim)
                .Where(x => userRoleIds.Contains(x.RoleId))
                .Select(x => new System.Security.Claims.Claim(CustomClaimTypeConsts.Claims, x.Claim.Name))
                .ToArrayAsync(cancellationToken);

            // Add manual claims
            var claims = new List<System.Security.Claims.Claim>();

            claims.AddRange(userClaims);

            return claims;
        }

        public async Task<List<System.Security.Claims.Claim>?> GetRoleClaimsByRoleId(int[] roleIds, CancellationToken cancellationToken)
        {
            // Get role claims for the user
            var userClaims = await _roleClaimRepository.Query()
                .Include(x => x.Claim)
                .Where(x => roleIds.Contains(x.RoleId))
                .Select(x => new System.Security.Claims.Claim(CustomClaimTypeConsts.Claims, x.Claim.Name))
                .ToArrayAsync(cancellationToken);

            // Add manual claims
            var claims = new List<System.Security.Claims.Claim>();

            claims.AddRange(userClaims);

            return claims;
        }
        #endregion

        #region commands

        public async Task<SignUpResponse> AddAsync(SignUpRequest model, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<UserEntity>(model);

            SetUserPassword(password: model.Password, user: entity);

            await _userManager.CreateAsync(entity);
            //await _userRepository.AddAsync(entity, cancellationToken: cancellationToken);

            var dto = _mapper.Map<SignUpResponse>(entity);

            return dto;
        }

        public async Task EditAsync(UserUpdateRequest model, CancellationToken cancellationToken)
        {
            var foundedEntity = await _userRepository.FindFirstAsync(x => x.Id == model.Id, cancellationToken);

            foundedEntity.ThrowIfNull(HttpResponseStatus.DomainUserNotFound);

            var entity = _mapper.Map(model, foundedEntity);

            SetUserPassword(
                password: model.Password,
                user: entity);

            foreach (var id in model.DeletedUserRoles)
            {
                var deleted = new UserRoleEntity { Id = id, IsDeleted = true };
                await _userRoleRepository.UpdateSpecificPropertiesAsync(deleted, false, false, x => x.IsDeleted);
            }

            await _userRepository.UpdateAsync(entity, cancellationToken: cancellationToken);
        }

        public async Task EditAuthTypeAsync(int id, AuthenticationTypeEnum authType, CancellationToken cancellationToken = default)
        {
            UserEntity entity = new() { Id = id, AuthenticationType = authType };

            await _userRepository.UpdateSpecificPropertiesAsync(entity, true, false, x => x.AuthenticationType);
        }

        public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken)
        {
            UserEntity entity = new() { Id = id, IsDeleted = true };

            await _userRepository.UpdateSpecificPropertiesAsync(entity, true, false, x => x.IsDeleted);
        }

        public async Task VerifyPasswordAsync(string password, UserDto currentUser, CancellationToken cancellationToken)
        {
            var hasher = new PasswordHasher<UserEntity>();
            var user = await _userRepository.FindAsync(currentUser.Id, cancellationToken);

            ThrowExceptionExtension.ThrowIfTrue((user == null || string.IsNullOrEmpty(user.PasswordHash)), HttpResponseStatus.DomainUserNotFound);

            var res = hasher.VerifyHashedPassword(
                user!,
                currentUser.PasswordHash,
                password);

            ThrowExceptionExtension.ThrowIfTrue(res != PasswordVerificationResult.Success, SharedResources.InvalidPassword);
        }

        public async Task UpdatePasswordAsync(UpdatePasswordRequest request)
        {
            ThrowExceptionExtension.ThrowIfTrue(request.Password != request.PasswordConfirm, SharedResources.InvalidRepeatedPassword);
            ThrowExceptionExtension.ThrowIfNull(request.OldPassword, SharedResources.RequireResetPassword);

            var passwordHasher = new PasswordHasher<UserEntity>();

            var entity = await _userManager.FindByIdAsync(request.UserId.ToString());

            var oldPasswordHash= passwordHasher.HashPassword(user: entity, password: request.OldPassword);

            ThrowExceptionExtension.ThrowIfTrue(entity.PasswordHash != oldPasswordHash, SharedResources.InvalidPassword);

            entity.PasswordHash = passwordHasher.HashPassword(user: entity, password: request.Password);

            await _userRepository.UpdateSpecificPropertiesAsync(entity, true, false, x => x.PasswordHash);
        }

        private static void SetUserPassword(string? password, UserEntity user)
        {
            if (!string.IsNullOrEmpty(password))
            {
                var passwordHasher = new PasswordHasher<UserEntity>();

                user.PasswordHash = passwordHasher.HashPassword(
                    user: user,
                    password: password);
            }
        }

        public async ValueTask<bool> ValidateRefreshTokenAsync(int userId, string refreshToken, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.Query(false).Where(user=> user.RefreshToken == refreshToken && user.Id == userId)
                               .FirstOrDefaultAsync(cancellationToken);

            ThrowExceptionExtension.ThrowIfNull(user, SharedResources.RefreshTokenNotFound);

            ThrowExceptionExtension.ThrowIfTrue(DateTime.UtcNow > user.TokenExpireDate, SharedResources.RefreshTokenExpired);

            return true;
        }

        public async Task<JwtTokenResult> UpdateRefreshTokenAsync(int userId, List<System.Security.Claims.Claim> claims, CancellationToken cancellationToken = default)
        {
            var result = _jwtService.GenerateToken(claims);
            var userEntity = new UserEntity()
            {
                Id = userId,
                RefreshToken = result.RefreshToken,
                TokenExpireDate = result.RefreshTokenExpiryDate,
                TokenCreatedDate = DateTime.UtcNow,
               
            };

            await _userRepository.UpdateSpecificPropertiesAsync(userEntity,
                true, false,
                x => x.RefreshToken,
                x => x.TokenExpireDate,
                x => x.TokenCreatedDate);
          
            return result;
        }

        #endregion

        #endregion
    }
}
