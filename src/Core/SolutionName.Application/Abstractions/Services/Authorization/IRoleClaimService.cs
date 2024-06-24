using SolutionName.Application.DTOs.Account.Authorization.RoleClaim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionName.Application.Abstractions.Services.Authorization
{
    public interface IRoleClaimService 
    {
        Task<List<RoleClaimTableResponse>> GetTableAsync();
        Task<List<RoleClaimDto>> GetTableByRoleIdAsync(int roleId);
        Task<List<RoleClaimDto>> GetTableByClaimIdAsync(int claimId);
        Task<RoleClaimUpdateRequest> GetForUpdateByIdAsync(int id);
        Task<RoleClaimDto> AddAsync(RoleClaimAddRequest model);
        Task EditAsync(RoleClaimUpdateRequest model);
        Task DeleteByIdAsync(int id);
        Task<RoleClaimDto> GetByIdAsync(int id);

    }
}
