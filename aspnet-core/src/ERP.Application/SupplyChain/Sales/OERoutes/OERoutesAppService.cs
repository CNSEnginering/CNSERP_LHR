using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Sales.OERoutes.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;
using ERP.Authorization.Users;

namespace ERP.SupplyChain.Sales.OERoutes
{
    [AbpAuthorize(AppPermissions.Sales_OERoutes)]
    public class OERoutesAppService : ERPAppServiceBase, IOERoutesAppService
    {
        private readonly IRepository<OERoutes> _oeRoutesRepository;
        private readonly IRepository<User, long> _userRepository;

        public OERoutesAppService(IRepository<OERoutes> oeRoutesRepository , IRepository<User, long> userRepository)
        {
            _oeRoutesRepository = oeRoutesRepository;
            _userRepository = userRepository;

        }

        public async Task<PagedResultDto<GetOERoutesForViewDto>> GetAll(GetAllOERoutesInput input)
        {

            var filteredOERoutes = _oeRoutesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.RoutID.ToString().Contains(input.Filter) || e.RoutDesc.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter))
                        .WhereIf(input.MinRoutIDFilter != null, e => e.RoutID >= input.MinRoutIDFilter)
                        .WhereIf(input.MaxRoutIDFilter != null, e => e.RoutID <= input.MaxRoutIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RoutDescFilter), e => e.RoutDesc == input.RoutDescFilter)
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter);

            var pagedAndFilteredOERoutes = filteredOERoutes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var oeRoutes = from o in pagedAndFilteredOERoutes
                           select new
                           {

                               o.RoutID,
                               o.RoutDesc,
                               o.Active,
                               o.CreatedBy,
                               o.CreateDate,
                               o.AudtUser,
                               o.AudtDate,
                               Id = o.Id
                           };

            var totalCount = await filteredOERoutes.CountAsync();

            var dbList = await oeRoutes.ToListAsync();
            var results = new List<GetOERoutesForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOERoutesForViewDto()
                {
                    OERoutes = new OERoutesDto
                    {

                        RoutID = o.RoutID,
                        RoutDesc = o.RoutDesc,
                        Active = o.Active,
                        CreatedBy = o.CreatedBy,
                        CreateDate = o.CreateDate,
                        AudtUser = o.AudtUser,
                        AudtDate = o.AudtDate,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOERoutesForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOERoutesForViewDto> GetOERoutesForView(int id)
        {
            var oeRoutes = await _oeRoutesRepository.GetAsync(id);

            var output = new GetOERoutesForViewDto { OERoutes = ObjectMapper.Map<OERoutesDto>(oeRoutes) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Sales_OERoutes_Edit)]
        public async Task<GetOERoutesForEditOutput> GetOERoutesForEdit(EntityDto input)
        {
            var oeRoutes = await _oeRoutesRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOERoutesForEditOutput { OERoutes = ObjectMapper.Map<CreateOrEditOERoutesDto>(oeRoutes) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOERoutesDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Sales_OERoutes_Create)]
        protected virtual async Task Create(CreateOrEditOERoutesDto input)
        {
            var oeRoutes = ObjectMapper.Map<OERoutes>(input);

            if (AbpSession.TenantId != null)
            {
                oeRoutes.TenantId = (int)AbpSession.TenantId;
                oeRoutes.CreatedBy = _userRepository.GetAll().Where(c => c.Id == AbpSession.UserId).SingleOrDefault().UserName;
                oeRoutes.CreateDate = DateTime.Now;
                
            }

            await _oeRoutesRepository.InsertAsync(oeRoutes);

        }

        [AbpAuthorize(AppPermissions.Sales_OERoutes_Edit)]
        protected virtual async Task Update(CreateOrEditOERoutesDto input)
        {
            var oeRoutes = await _oeRoutesRepository.FirstOrDefaultAsync((int)input.Id);
            input.AudtUser = _userRepository.GetAll().Where(c => c.Id == AbpSession.UserId).SingleOrDefault().UserName;
            input.AudtDate = DateTime.Now;
            ObjectMapper.Map(input, oeRoutes);

        }

        [AbpAuthorize(AppPermissions.Sales_OERoutes_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _oeRoutesRepository.DeleteAsync(input.Id);
        }

        public int GetMaxDocNo()
        {
            int routid = 0;
            return routid = ((from tab1 in _oeRoutesRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.RoutID).Max() ?? 0) + 1;
        }


    }
}