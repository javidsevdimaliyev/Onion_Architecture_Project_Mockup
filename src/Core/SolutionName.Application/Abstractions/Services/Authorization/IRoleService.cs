using SolutionName.Application.DTOs.Account.Authorization.Role;
using SolutionName.Application.Models.Requests;

namespace SolutionName.Application.Abstractions.Services.Authorization
{
    public interface IRoleService
    {
        Task<List<DropDownDto>> GetListAsync(CancellationToken cancellationToken = default);
        Task<RoleTableListResponse> GetTableAsync(PagingRequest pagingRequest);
        Task<RoleUpdateRequest> GetForUpdateByIdAsync(int id);
        Task<RoleDto> AddAsync(RoleAddRequest model);
        Task EditAsync(RoleUpdateRequest model);
        Task DeleteByIdAsync(int id);
        Task<RoleDto> GetByIdAsync(int id);

        Task<List<RoleDto>> GetRolesByUserIdAsync(
            int userId,
            CancellationToken cancellationToken = default);
    }
}
