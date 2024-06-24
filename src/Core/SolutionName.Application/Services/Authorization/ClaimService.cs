using AutoMapper;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using SolutionName.Application.Abstractions.Repositories.EntityFramework;
using SolutionName.Application.Abstractions.Services.Authorization;
using SolutionName.Application.DTOs.Account.Authorization.Claim;
using SolutionName.Application.Models.Requests;
using SolutionName.Application.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SolutionName.Application.Services.Authorization
{
    public class ClaimService : IClaimService
    {
        private readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IRepository<ClaimEntity, int> _repository;


        public ClaimService(IMapper mapper, IUnitOfWork unitOfWork) 
        {
            _mapper = mapper;
            _repository = unitOfWork.Repository<ClaimEntity, int>(); ;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<DropDownDto>> GetListAsync(CancellationToken cancellationToken) =>
           await _repository.Query().Select(x => new DropDownDto
           {
               Value = x.Id,
               DisplayText = $"page: {x.Page}/ module: {x.Module}/ claim: {x.Name}"
           }).ToListAsync(cancellationToken: cancellationToken);

        public async Task<List<ClaimHierarchDto>> GetHierarchListAsync(CancellationToken cancellationToken)
        {
            var claims = await _repository.FindAllAsync();
            var distinctModules = claims.Select(m => m.Module).Distinct();

            var list = new List<ClaimHierarchDto>();
            foreach (var claimModule in distinctModules)
            {
                var distinctPages = claims.Where(x => x.Module == claimModule).Select(m => m.Page).Distinct();

                var hierarch = new ClaimHierarchDto();
                hierarch.Module = claimModule;
                hierarch.Pages = new List<ClaimHierarchForPageDto>();


                foreach (var page in distinctPages)
                {

                    var hierarchPage = new ClaimHierarchForPageDto();
                    hierarchPage.Claims = new List<ClaimDto>();

                    var pageclaims = claims.Where(x => x.Page == page && x.Module == claimModule).ToList();
                    foreach (var pageclaim in pageclaims)
                    {
                        hierarchPage.Page = pageclaim.Page;

                        hierarchPage.Claims.Add(new ClaimDto()
                        {
                            Name = pageclaim.Name,
                            Value = pageclaim.Name,
                            Module = pageclaim.Module,
                            Page = pageclaim.Page,
                            DisplayOrder = pageclaim.OrderIndex,
                            IsDeleted = pageclaim.IsDeleted,
                            Id = pageclaim.Id
                        });
                    }

                    hierarch.Pages.Add(hierarchPage);


                }

                list.Add(hierarch);
            }

            return list;
        }



        public async Task<ClaimTableListResponse> GetTableAsync(PagingRequest pagingRequest)
        {
            var query = from o in _repository.Query()

                        select new ClaimTableResponse
                        {
                            Name = o.Name,
                            Value = o.Name,
                            Module = o.Module,
                            Page = o.Page,
                            DisplayOrder = o.OrderIndex,
                            IsDeleted = o.IsDeleted,
                            Id = o.Id,

                        };

            query = query.AddFilter(pagingRequest, out int count);

            return new ClaimTableListResponse { Count = count, Data = await query.ToListAsync() };
        }

        public async Task<ClaimUpdateRequest> GetForUpdateByIdAsync(int id)
        {
            var entity = await _repository.FindByIdAsync(id);
            var model = _mapper.Map<ClaimUpdateRequest>(entity);

            return model;
        }
        public async Task<ClaimDto> AddAsync(LoginRequest model)
        {
            var entity = _mapper.Map<ClaimEntity>(model);

            await _repository.AddAsync(entity);

            var Dto = _mapper.Map<ClaimDto>(entity);

            return Dto;
        }

        public async Task EditAsync(ClaimUpdateRequest model)
        {
            var entity = _mapper.Map<ClaimEntity>(model);

             await _repository.UpdateAsync(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            ClaimEntity entity = new() { Id = id, IsDeleted = true };

            await _repository.UpdateSpecificPropertiesAsync(entity, true, false, x => x.IsDeleted);
        }

        public async Task<ClaimDto> GetByIdAsync(int id)
        {
            var entity = await _repository.FindByIdAsync(id);

            return _mapper.Map<ClaimDto>(entity);
        }
    }
}
