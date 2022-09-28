using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory.ICLOT.Exporting;
using ERP.SupplyChain.Inventory.ICLOT.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;
using ERP.Authorization.Users;

namespace ERP.SupplyChain.Inventory.ICLOT
{
    [AbpAuthorize(AppPermissions.Pages_ICLOT)]
    public class ICLOTAppService : ERPAppServiceBase, IICLOTAppService
    {
        private readonly IRepository<ICLOT> _iclotRepository;
        private readonly IICLOTExcelExporter _iclotExcelExporter;
        private readonly IRepository<User, long> _userRepository;
        public ICLOTAppService(IRepository<User, long> userRepository, IRepository<ICLOT> iclotRepository, IICLOTExcelExporter iclotExcelExporter)
        {
            _iclotRepository = iclotRepository;
            _iclotExcelExporter = iclotExcelExporter;
            _userRepository = userRepository;
        }

        public async Task<PagedResultDto<GetICLOTForViewDto>> GetAll(GetAllICLOTInput input)
        {
            var tenantid = AbpSession.TenantId;
            var filteredICLOT = _iclotRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LotNo.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(tenantid != null, e => e.TenantId >= tenantid)
                        .WhereIf(tenantid != null, e => e.TenantId <= tenantid)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LotNoFilter), e => e.LotNo == input.LotNoFilter)
                        .WhereIf(input.MinManfDateFilter != null, e => e.ManfDate >= input.MinManfDateFilter)
                        .WhereIf(input.MaxManfDateFilter != null, e => e.ManfDate <= input.MaxManfDateFilter)
                        .WhereIf(input.MinExpiryDateFilter != null, e => e.ExpiryDate >= input.MinExpiryDateFilter)
                        .WhereIf(input.MaxExpiryDateFilter != null, e => e.ExpiryDate <= input.MaxExpiryDateFilter)
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredICLOT = filteredICLOT
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var iclot = from o in pagedAndFilteredICLOT
                        select new
                        {

                            o.TenantId,
                            o.LotNo,
                            o.ManfDate,
                            o.ExpiryDate,
                            o.Active,
                            o.AudtUser,
                            o.AudtDate,
                            o.CreatedBy,
                            o.CreateDate,
                            Id = o.Id
                        };

            var totalCount = await filteredICLOT.CountAsync();

            var dbList = await iclot.ToListAsync();
            var results = new List<GetICLOTForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetICLOTForViewDto()
                {
                    ICLOT = new ICLOTDto
                    {

                        //TenantId = o.TenantId,
                        LotNo = o.LotNo,
                        ManfDate = o.ManfDate,
                        ExpiryDate = o.ExpiryDate,
                        Active = o.Active,
                        AudtUser = o.AudtUser,
                        AudtDate = o.AudtDate,
                        CreatedBy = o.CreatedBy,
                        CreateDate = o.CreateDate,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetICLOTForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetICLOTForViewDto> GetICLOTForView(int id)
        {
            var iclot = await _iclotRepository.GetAsync(id);

            var output = new GetICLOTForViewDto { ICLOT = ObjectMapper.Map<ICLOTDto>(iclot) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ICLOT_Edit)]
        public async Task<GetICLOTForEditOutput> GetICLOTForEdit(EntityDto input)
        {
            var iclot = await _iclotRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetICLOTForEditOutput { ICLOT = ObjectMapper.Map<CreateOrEditICLOTDto>(iclot) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditICLOTDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ICLOT_Create)]
        protected virtual async Task Create(CreateOrEditICLOTDto input)
        {
            try
            {
                var iclot = ObjectMapper.Map<ICLOT>(input);

                if (AbpSession.TenantId != null)
                {
                    iclot.TenantId = (int)AbpSession.TenantId;
                    iclot.CreateDate =DateTime.Now;
                    iclot.CreatedBy= _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                  
                }

                await _iclotRepository.InsertAsync(iclot);
            }
            catch (Exception ex)
            {
                
            }
          

        }

        [AbpAuthorize(AppPermissions.Pages_ICLOT_Edit)]
        protected virtual async Task Update(CreateOrEditICLOTDto input)
        {
            input.AudtDate = DateTime.Now;
            input.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;

            var iclot = await _iclotRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, iclot);

        }

        [AbpAuthorize(AppPermissions.Pages_ICLOT_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _iclotRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetICLOTToExcel(GetAllICLOTForExcelInput input)
        {
            var tenantid = AbpSession.TenantId;
            var filteredICLOT = _iclotRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LotNo.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(tenantid != null, e => e.TenantId >= tenantid)
                        .WhereIf(tenantid != null, e => e.TenantId <= tenantid)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LotNoFilter), e => e.LotNo == input.LotNoFilter)
                        .WhereIf(input.MinManfDateFilter != null, e => e.ManfDate >= input.MinManfDateFilter)
                        .WhereIf(input.MaxManfDateFilter != null, e => e.ManfDate <= input.MaxManfDateFilter)
                        .WhereIf(input.MinExpiryDateFilter != null, e => e.ExpiryDate >= input.MinExpiryDateFilter)
                        .WhereIf(input.MaxExpiryDateFilter != null, e => e.ExpiryDate <= input.MaxExpiryDateFilter)
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredICLOT
                         select new GetICLOTForViewDto()
                         {
                             ICLOT = new ICLOTDto
                             {
                                // TenantId = o.TenantId,
                                 LotNo = o.LotNo,
                                 ManfDate = o.ManfDate,
                                 ExpiryDate = o.ExpiryDate,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });

            var iclotListDtos = await query.ToListAsync();

            return _iclotExcelExporter.ExportToFile(iclotListDtos);
        }

    }
}