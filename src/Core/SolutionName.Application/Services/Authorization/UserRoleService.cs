using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SolutionName.Application.Abstractions.Repositories.EntityFramework;
using SolutionName.Application.Abstractions.Services.Authorization;
using SolutionName.Application.DTOs.Account.Authorization.UserRole;
using SolutionName.Application.Utilities.Helpers;

namespace SolutionName.Application.Services.Authorization
{
    public class UserRoleService : IUserRoleService
    {
        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly UserManager<UserEntity> _userManager;
        readonly IRepository<UserRoleEntity, int> _repository;
        readonly IRepository<UserEntity, int> _userRepository;
        readonly IRepository<RoleEntity, int> _roleRepository;
        public UserRoleService(IUnitOfWork unitOfWork,
                    UserManager<UserEntity> userManager,
                    IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userRepository = unitOfWork.Repository<UserEntity, int>();
            _repository = unitOfWork.Repository<UserRoleEntity, int>();
            _mapper = mapper;
        }
        public async Task AssignRoleToUserAsnyc(string userId, string[] roles)
        {
            UserEntity user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);

                await _userManager.AddToRolesAsync(user, roles);
            }
        }

        public async Task<string[]> GetRolesToUserAsync(string userIdOrName)
        {
            UserEntity user = await _userManager.FindByIdAsync(userIdOrName);
            if (user == null)
                user = await _userManager.FindByNameAsync(userIdOrName);

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                return userRoles.ToArray();
            }
            return new string[] { };
        }

        public async Task<bool> HasRolePermissionToEndpointAsync(string roleName)
        {
            var currentUserRoles = await _userRepository.Query()
                .Where(x => x.Id == IdentityHelper.UserId)
                .Include(i => i.UserRoles)
                  .ThenInclude(t=>t.Role)
                .SelectMany(x => x.UserRoles)
                .Select(ur => ur.Role.Name).ToListAsync();
            
            return currentUserRoles.Contains(roleName);
        }

        public async Task<List<UserRoleDto>> GetTableByRoleIdAsync(int roleId)
        {
            var res = await _repository.Query().Where(x => x.RoleId == roleId).Select(entity => new UserRoleDto
            {
                RoleId = entity.RoleId,
                UserId = entity.UserId,
                IsDeleted = entity.IsDeleted,
                Id = entity.Id,
            }).ToListAsync();

            return res;
        }
        public Task<List<UserRoleDto>> GetTableByUserIdAsync(int userId)
        {
            return _repository.Query().Where(x => x.UserId == userId).Select(entity => new UserRoleDto
            {
                RoleId = entity.RoleId,
                UserId = entity.UserId,
                IsDeleted = entity.IsDeleted,
                Id = entity.Id,
            }).ToListAsync();

        }
        public Task<List<UserRoleTableResponse>> GetTableAsync()
        {
            var query = from o in _repository.Query()
                        join b in _roleRepository.Query() on o.RoleId equals b.Id
                        join c in _userRepository.Query() on o.UserId equals c.Id
                        select new UserRoleTableResponse
                        {
                            RoleId = o.RoleId,
                            UserId = o.UserId,
                            IsDeleted = o.IsDeleted,
                            Id = o.Id,
                            RoleName = b.Name,
                            UserName = c.Name,

                        };

            return query.ToListAsync();
        }

        public async Task<UserRoleUpdateRequest> GetForUpdateByIdAsync(int id)
        {
            var entity = await _repository.FindAsync(id);
            var model = _mapper.Map<UserRoleUpdateRequest>(entity);

            return model;
        }
        public async Task<UserRoleDto> AddAsync(UserRoleAddRequest model)
        {
            var entity = _mapper.Map<UserRoleEntity>(model);

            await _repository.AddAsync(entity);

            var Dto = _mapper.Map<UserRoleDto>(entity);

            return Dto;
        }

        public async Task EditAsync(UserRoleUpdateRequest model)
        {
            var entity = _mapper.Map<UserRoleEntity>(model);

            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            UserRoleEntity entity = new() { Id = id, IsDeleted = true };

            await _repository.UpdateSpecificPropertiesAsync(entity, true, false, x => x.IsDeleted);
        }

        public async Task<UserRoleDto> GetByIdAsync(int id)
        {
            var entity = await _repository.FindAsync(id);

            return _mapper.Map<UserRoleDto>(entity);
        }
    }
}
