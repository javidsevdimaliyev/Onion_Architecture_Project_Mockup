using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SolutionName.Application.Abstractions.Repositories.EntityFramework;
using SolutionName.Application.Abstractions.Services.Authorization;
using SolutionName.Application.DTOs.Account.Authorization.Role;
using SolutionName.Application.DTOs.Account.Authorization.RoleClaim;
using SolutionName.Application.Models.Requests;
using SolutionName.Application.Utilities.Extensions;
using System.Data;

namespace SolutionName.Application.Services.Authorization
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IRepository<RoleEntity, int> _repository;
        readonly IRepository<UserRoleEntity, int> _userRoleRepository;
        readonly IRepository<RoleClaimEntity, int> _roleClaimRepository;
        public RoleService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = unitOfWork.Repository<RoleEntity, int>();
            _roleClaimRepository = unitOfWork.Repository<RoleClaimEntity, int>(); ;
            _userRoleRepository = unitOfWork.Repository<UserRoleEntity, int>(); ;
           
        }

        public async Task<List<RoleDto>> GetRolesByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var roles = await (from role in _repository.Query()
                               join userRole in _userRoleRepository.Query() on role.Id equals userRole.RoleId
                               where userRole.UserId == userId
                               select new RoleDto { Name = role.Name, Id = role.Id }
                   ).ToListAsync(cancellationToken: cancellationToken);

            return roles;
        }

        public async Task<RoleTableListResponse> GetTableAsync(PagingRequest pagingRequest)
        {
            var query = from o in _repository.Query()

                        select new RoleTableResponse
                        {
                            Name = o.Name,
                            DisplayOrder = o.OrderIndex,
                            IsDeleted = o.IsDeleted,
                            Id = o.Id,

                        };

            query = query.AddFilter(pagingRequest, out int count);
  
            return new RoleTableListResponse { Count = count, Data = await query.ToListAsync() };
        }

        public async Task<RoleUpdateRequest> GetForUpdateByIdAsync(int id)
        {
            var entity = await _repository.Query().Include(x => x.RoleClaims).ThenInclude(x => x.Claim).Select(x => new RoleUpdateRequest
            {
                Id = x.Id,
                IsDeleted = x.IsDeleted,
                Name = x.Name,
                DisplayOrder = x.OrderIndex,
                RoleClaims = x.RoleClaims.Select(y => new RoleClaimUpdateRequest
                {
                    Id = y.Id,
                    IsDeleted = y.IsDeleted,
                    ClaimId = y.ClaimId,
                    RoleId = y.RoleId,
                    ClaimName = y.Claim.Name,
                    ClaimDisplayName = y.Claim.Name,

                }).ToList()
            }).FirstOrDefaultAsync(x => x.Id == id);


            return entity;
        }

        public Task<List<DropDownDto>> GetListAsync(CancellationToken cancellationToken) =>
            _repository.Query().Select(x => new DropDownDto
            {
                Value = x.Id,
                DisplayText = x.Name
            }).ToListAsync(cancellationToken: cancellationToken);

        public async Task<RoleDto> AddAsync(RoleAddRequest model)
        {
            var entity = _mapper.Map<RoleEntity>(model);

            await _repository.AddAsync(entity);

            var Dto = _mapper.Map<RoleDto>(entity);

            return Dto;
        }

        public async Task EditAsync(RoleUpdateRequest model)
        {
            var entity = _mapper.Map<RoleEntity>(model);

            foreach (var id in model.DeletedRoleClaims)
            {
                var deleted = await _roleClaimRepository.FindAsync(id);
                deleted.IsDeleted = true;
                await _roleClaimRepository.UpdateAsync(deleted, false);
            }
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            RoleEntity entity = new() { Id = id, IsDeleted = true };

            await _repository.UpdateSpecificPropertiesAsync(entity, true, false, x => x.IsDeleted);
        }

        public async Task<RoleDto> GetByIdAsync(int id)
        {
            var entity = await _repository.FindAsync(id);

            return _mapper.Map<RoleDto>(entity);
        }
    }
}
