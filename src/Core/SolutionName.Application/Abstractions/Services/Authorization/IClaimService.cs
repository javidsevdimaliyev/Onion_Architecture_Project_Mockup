using Microsoft.AspNetCore.Identity.Data;
using SolutionName.Application.DTOs.Account.Authorization.Claim;
using SolutionName.Application.Models.Requests;

namespace SolutionName.Application.Abstractions.Services.Authorization
{
    public interface IClaimService 
    {
        Task<List<DropDownDto>> GetListAsync(CancellationToken cancellationToken = default);
        Task<List<ClaimHierarchDto>> GetHierarchListAsync(CancellationToken cancellationToken);
        Task<ClaimTableListResponse> GetTableAsync(PagingRequest pagingRequest);
        Task<ClaimUpdateRequest> GetForUpdateByIdAsync(int id);
        Task<ClaimDto> AddAsync(LoginRequest model);
        Task EditAsync(ClaimUpdateRequest model);
        Task DeleteByIdAsync(int id);
        Task<ClaimDto> GetByIdAsync(int id);

    }
}
