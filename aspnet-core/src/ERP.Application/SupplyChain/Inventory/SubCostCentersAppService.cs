

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.Authorization.Users;
using ERP.SupplyChain.Inventory.Exporting;
using ERP.GeneralLedger.SetupForms;

namespace ERP.SupplyChain.Inventory
{
    [AbpAuthorize(AppPermissions.Inventory_SubCostCenters)]
    public class SubCostCentersAppService : ERPAppServiceBase, ISubCostCentersAppService
    {
        private readonly IRepository<SubCostCenter> _subCostCenterRepository;
        private readonly IRepository<CostCenter> _costCenterRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly ISubCostCenterExcelExporter _ISubCostCenterExcelExporter;
        private readonly IRepository<ChartofControl, string> _accountRepository;
        public SubCostCentersAppService(IRepository<SubCostCenter> subCostCenterRepository,
            IRepository<CostCenter> costCenterRepository,
            IRepository<User, long> userRepository,
            ISubCostCenterExcelExporter ISubCostCenterExcelExporter,
            IRepository<ChartofControl, string> accountRepository)
        {
            _subCostCenterRepository = subCostCenterRepository;
            _costCenterRepository = costCenterRepository;
            _userRepository = userRepository;
            _ISubCostCenterExcelExporter = ISubCostCenterExcelExporter;
            _accountRepository = accountRepository;
        }

        public async Task<PagedResultDto<GetSubCostCenterForViewDto>> GetAll(GetAllSubCostCentersInput input)
        {

            var filteredSubCostCenters = _subCostCenterRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CCID.Contains(input.Filter) || e.SubCCName.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CCIDFilter), e => e.CCID == input.CCIDFilter)
                        .WhereIf(input.MinSUBCCIDFilter != null, e => e.SUBCCID >= input.MinSUBCCIDFilter)
                        .WhereIf(input.MaxSUBCCIDFilter != null, e => e.SUBCCID <= input.MaxSUBCCIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubCCNameFilter), e => e.SubCCName == input.SubCCNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
                        .Where(o => o.TenantId == AbpSession.TenantId);

            var pagedAndFilteredSubCostCenters = filteredSubCostCenters
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var subCostCenters = from o in pagedAndFilteredSubCostCenters
                                 select new GetSubCostCenterForViewDto()
                                 {
                                     SubCostCenter = new SubCostCenterDto
                                     {
                                         CCID = o.CCID,
                                         SUBCCID = o.SUBCCID,
                                         SubCCName = o.SubCCName,
                                         AudtUser = o.AudtUser,
                                         AudtDate = o.AudtDate,
                                         CreatedBy = o.CreatedBy,
                                         CreateDate = o.CreateDate,
                                         CCName = _costCenterRepository.GetAll().Where(p => p.CCID == o.CCID).SingleOrDefault().CCName,
                                         Id = o.Id,
                                        Active=o.Active,
                                         AccountId = o.AccountID,
                                         AccountName = _accountRepository.GetAll().Where(a => a.Id == o.AccountID).Count() > 0 ?
                                         _accountRepository.GetAll().Where(a => a.Id == o.AccountID).FirstOrDefault().AccountName : ""
                                     }
                                 };

            var totalCount = await filteredSubCostCenters.CountAsync();

            return new PagedResultDto<GetSubCostCenterForViewDto>(
                totalCount,
                await subCostCenters.ToListAsync()
            );
        }

        public async Task<GetSubCostCenterForViewDto> GetSubCostCenterForView(int id)
        {
            var subCostCenter = await _subCostCenterRepository.GetAsync(id);

            var output = new GetSubCostCenterForViewDto { SubCostCenter = ObjectMapper.Map<SubCostCenterDto>(subCostCenter) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Inventory_SubCostCenters_Edit)]
        public async Task<GetSubCostCenterForEditOutput> GetSubCostCenterForEdit(EntityDto input)
        {
            var subCostCenter = await _subCostCenterRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSubCostCenterForEditOutput { SubCostCenter = ObjectMapper.Map<CreateOrEditSubCostCenterDto>(subCostCenter) };
            output.SubCostCenter.CCName = _costCenterRepository.GetAll().Where(o => o.CCID == output.SubCostCenter.CCID).FirstOrDefault().CCName;
            output.SubCostCenter.AccountName = _accountRepository.GetAll().Where(a => a.Id == output.SubCostCenter.AccountId).Count() > 0 ?
            _accountRepository.GetAll().Where(a => a.Id == output.SubCostCenter.AccountId).FirstOrDefault().AccountName : "";
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSubCostCenterDto input)
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

        [AbpAuthorize(AppPermissions.Inventory_SubCostCenters_Create)]
        protected virtual async Task Create(CreateOrEditSubCostCenterDto input)
        {
            var subCostCenter = ObjectMapper.Map<SubCostCenter>(input);


            if (AbpSession.TenantId != null)
            {
                subCostCenter.TenantId = Convert.ToInt32(AbpSession.TenantId);
            }

            subCostCenter.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            subCostCenter.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            subCostCenter.AudtDate = DateTime.Now;
            subCostCenter.CreateDate = DateTime.Now;
            await _subCostCenterRepository.InsertAsync(subCostCenter);
        }

        [AbpAuthorize(AppPermissions.Inventory_SubCostCenters_Edit)]
        protected virtual async Task Update(CreateOrEditSubCostCenterDto input)
        {
            var subCostCenter = await _subCostCenterRepository.FirstOrDefaultAsync((int)input.Id);
            input.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            input.AudtDate = DateTime.Now;
            input.CreatedBy = subCostCenter.CreatedBy;
            input.CreateDate = subCostCenter.CreateDate;
            ObjectMapper.Map(input, subCostCenter);
        }

        [AbpAuthorize(AppPermissions.Inventory_SubCostCenters_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _subCostCenterRepository.DeleteAsync(input.Id);
        }

        public int GetSubCostCenterId(string CCId)
        {
            var subCostCenter = _subCostCenterRepository.GetAll().Where(o => o.CCID == CCId && o.TenantId == AbpSession.TenantId);
            return subCostCenter.Count() > 0 ? (subCostCenter.Max(o => o.SUBCCID) + 1) : 1;
        }

        [AbpAuthorize(AppPermissions.Inventory_SubCostCenters)]
        public async Task<PagedResultDto<SubCostCenterDto>> GetAllSubCostCenterForLookupTable(GetAllSubCostCentersInput input)
        {
            var query = _subCostCenterRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CCID.Trim().Contains(input.Filter) || e.SubCCName.Trim().Contains(input.Filter))
                .Where(o => o.TenantId == AbpSession.TenantId && o.CCID.Trim() == input.CCIDFilter.Trim());

            var totalCount = await query.CountAsync();

            var sublCostList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<SubCostCenterDto>();
            foreach (var sublCost in sublCostList)
            {
                lookupTableDtoList.Add(new SubCostCenterDto
                {
                    SUBCCID = sublCost.SUBCCID,
                    SubCCName = sublCost.SubCCName
                });
            }

            return new PagedResultDto<SubCostCenterDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<FileDto> GetDataToExcel(GetAllSubCostCentersInput input)
        {

            var filteredSubCostCenters = _subCostCenterRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CCID.Contains(input.Filter) || e.SubCCName.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CCIDFilter), e => e.CCID == input.CCIDFilter)
                        .WhereIf(input.MinSUBCCIDFilter != null, e => e.SUBCCID >= input.MinSUBCCIDFilter)
                        .WhereIf(input.MaxSUBCCIDFilter != null, e => e.SUBCCID <= input.MaxSUBCCIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubCCNameFilter), e => e.SubCCName == input.SubCCNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredSubCostCenters
                         select new GetSubCostCenterForViewDto()
                         {
                             SubCostCenter = new SubCostCenterDto
                             {
                                 CCID = o.CCID,
                                 SUBCCID = o.SUBCCID,
                                 SubCCName = o.SubCCName,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 CCName = _costCenterRepository.GetAll().Where(p => p.CCID == o.CCID).SingleOrDefault().CCName,
                                 Id = o.Id
                             }
                         });


            var dataDto = await query.ToListAsync();

            return _ISubCostCenterExcelExporter.ExportToFile(dataDto);
        }
    }
}