using SolutionName.Application.DTOs.Account.Authorization.UserRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionName.Application.Abstractions.Services.Authorization
{
    public interface IUserRoleService
    {
        Task AssignRoleToUserAsnyc(string userId, string[] roles);
        Task<string[]> GetRolesToUserAsync(string userIdOrName);
        Task<bool> HasRolePermissionToEndpointAsync(string roleName);
        Task<List<UserRoleTableResponse>> GetTableAsync();
        Task<List<UserRoleDto>> GetTableByRoleIdAsync(int roleId);
        Task<List<UserRoleDto>> GetTableByUserIdAsync(int userId);
        Task<UserRoleUpdateRequest> GetForUpdateByIdAsync(int id);
        Task<UserRoleDto> AddAsync(UserRoleAddRequest model);
        Task EditAsync(UserRoleUpdateRequest model);
        Task DeleteByIdAsync(int id);
        Task<UserRoleDto> GetByIdAsync(int id);
    }
}
