using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SolutionName.Application.Abstractions.Repositories.EntityFramework;
using SolutionName.Application.Abstractions.Services.Authorization;
using SolutionName.Application.DTOs.Account.Authorization.RoleClaim;

namespace SolutionName.Application.Services.Authorization
{
    public class RoleClaimService: IRoleClaimService
    {
        private readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IRepository<RoleClaimEntity, int> _repository;
        readonly IRepository<RoleEntity, int> _roleRepository;
        readonly IRepository<ClaimEntity, int> _claimRepository;

        public RoleClaimService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = unitOfWork.Repository<RoleClaimEntity, int>(); 
            _roleRepository = unitOfWork.Repository<RoleEntity, int>(); 
            _claimRepository = unitOfWork.Repository<ClaimEntity, int>();            
        }

        public Task<List<RoleClaimDto>> GetTableByRoleIdAsync(int roleId)
        {
            return _repository.Query().Where(x => x.RoleId == roleId).Select(entity => new RoleClaimDto
            {
                RoleId = entity.RoleId,
                ClaimId = entity.ClaimId,
                IsDeleted = entity.IsDeleted,
                Id = entity.Id,


            }).ToListAsync();

        }
        public Task<List<RoleClaimDto>> GetTableByClaimIdAsync(int claimId)
        {
            return _repository.Query().Where(x => x.ClaimId == claimId).Select(entity => new RoleClaimDto
            {
                RoleId = entity.RoleId,
                ClaimId = entity.ClaimId,
                IsDeleted = entity.IsDeleted,
                Id = entity.Id,


            }).ToListAsync();

        }
        public Task<List<RoleClaimTableResponse>> GetTableAsync()
        {
            var query = from o in _repository.Query()
                        join b in _roleRepository.Query() on o.RoleId equals b.Id
                        join c in _claimRepository.Query() on o.ClaimId equals c.Id
                        select new RoleClaimTableResponse
                        {
                            RoleId = o.RoleId,
                            ClaimId = o.ClaimId,
                            IsDeleted = o.IsDeleted,
                            Id = o.Id,
                            RoleName = b.Name,
                            ClaimName = c.Name,

                        };

            return query.ToListAsync();
        }

        public async Task<RoleClaimUpdateRequest> GetForUpdateByIdAsync(int id)
        {
            var entity = await _repository.FindAsync(id);
            var model = _mapper.Map<RoleClaimUpdateRequest>(entity);

            return model;
        }
        public async Task<RoleClaimDto> AddAsync(RoleClaimAddRequest model)
        {
            var entity = _mapper.Map<RoleClaimEntity>(model);

            await _repository.AddAsync(entity);

            var Dto = _mapper.Map<RoleClaimDto>(entity);

            return Dto;
        }

        public async Task EditAsync(RoleClaimUpdateRequest model)
        {
            var entity = _mapper.Map<RoleClaimEntity>(model);

            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            RoleClaimEntity entity = new() { Id = id, IsDeleted = true };

            await _repository.UpdateSpecificPropertiesAsync(entity, true, false, x => x.IsDeleted);
        }

        public async Task<RoleClaimDto> GetByIdAsync(int id)
        {
            var entity = await _repository.FindByIdAsync(id);

            return _mapper.Map<RoleClaimDto>(entity);
        }

    }
}
