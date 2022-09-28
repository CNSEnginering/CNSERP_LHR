

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
using ERP.GeneralLedger.SetupForms;
using ERP.Authorization.Users;
using ERP.SupplyChain.Inventory.Exporting;

namespace ERP.SupplyChain.Inventory
{
    [AbpAuthorize(AppPermissions.Inventory_CostCenters)]
    public class CostCentersAppService : ERPAppServiceBase, ICostCentersAppService
    {
        private readonly IRepository<CostCenter> _costCenterRepository;
        private readonly IRepository<ChartofControl, string> _chartOfControlRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly ICostCenterExcelExporter _ICostCenterExcelExporter;
        public CostCentersAppService(IRepository<CostCenter> costCenterRepository,
            IRepository<ChartofControl, string> chartOfControlRepository,
            IRepository<AccountSubLedger> accountSubLedgerRepository,
            IRepository<User, long> userRepository,
            ICostCenterExcelExporter ICostCenterExcelExporter)
        {
            _ICostCenterExcelExporter = ICostCenterExcelExporter;
            _costCenterRepository = costCenterRepository;
            _chartOfControlRepository = chartOfControlRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _userRepository = userRepository;
        }

        public async Task<PagedResultDto<GetCostCenterForViewDto>> GetAll(GetAllCostCentersInput input)
        {

            var filteredCostCenters = _costCenterRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CCID.Contains(input.Filter) || e.CCName.Contains(input.Filter) || e.AccountID.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CCIDFilter), e => e.CCID == input.CCIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CCNameFilter), e => e.CCName == input.CCNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.AccountID == input.AccountIDFilter)
                        .WhereIf(input.MinSubAccIDFilter != null, e => e.SubAccID >= input.MinSubAccIDFilter)
                        .WhereIf(input.MaxSubAccIDFilter != null, e => e.SubAccID <= input.MaxSubAccIDFilter)
                        //.WhereIf(input.MinActiveFilter != null, e => e.Active >= input.MinActiveFilter)
                        //.WhereIf(input.MaxActiveFilter != null, e => e.Active <= input.MaxActiveFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
                        .Where(o => o.TenantId == AbpSession.TenantId).WhereIf(input.MaxActiveFilter != null, e => e.Active == input.MaxActiveFilter);

            var pagedAndFilteredCostCenters = filteredCostCenters
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var costCenters = from o in pagedAndFilteredCostCenters
                              select new GetCostCenterForViewDto()
                              {
                                  CostCenter = new CostCenterDto
                                  {
                                      CCID = o.CCID,
                                      CCName = o.CCName,
                                      AccountID = o.AccountID,
                                      AccountName = _chartOfControlRepository.GetAll().Where(p => p.Id == o.AccountID && o.TenantId == AbpSession.TenantId).SingleOrDefault().AccountName,
                                      SubAccID = o.SubAccID,
                                      SubAccName = o.SubAccID > 0 ? _accountSubLedgerRepository.GetAll().Where(p => p.AccountID == o.AccountID && p.Id == o.SubAccID && o.TenantId == AbpSession.TenantId).SingleOrDefault().SubAccName : "",
                                      Active = o.Active,
                                      AudtUser = o.AudtUser,
                                      AudtDate = o.AudtDate,
                                      CreatedBy = o.CreatedBy,
                                      CreateDate = o.CreateDate,
                                      Id = o.Id
                                  }
                              };

            var totalCount = await filteredCostCenters.CountAsync();

            return new PagedResultDto<GetCostCenterForViewDto>(
                totalCount,
                await costCenters.ToListAsync()
            );
        }

        public async Task<GetCostCenterForViewDto> GetCostCenterForView(int id)
        {
            var costCenter = await _costCenterRepository.GetAsync(id);

            var output = new GetCostCenterForViewDto { CostCenter = ObjectMapper.Map<CostCenterDto>(costCenter) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Inventory_CostCenters_Edit)]
        public async Task<GetCostCenterForEditOutput> GetCostCenterForEdit(EntityDto input)
        {
            var costCenter = await _costCenterRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCostCenterForEditOutput { CostCenter = ObjectMapper.Map<CreateOrEditCostCenterDto>(costCenter) };
            output.CostCenter.AccountName = _chartOfControlRepository.GetAll().Where(o => o.Id == output.CostCenter.AccountID).Count() > 0 ?
                _chartOfControlRepository.GetAll().Where(o => o.Id == output.CostCenter.AccountID).FirstOrDefault().AccountName : "";
            output.CostCenter.SubAccName = _accountSubLedgerRepository.GetAll().Where(o => o.Id == output.CostCenter.SubAccID).Count() > 0 ?
                _accountSubLedgerRepository.GetAll().Where(o => o.Id == output.CostCenter.SubAccID).FirstOrDefault().SubAccName :
                "";
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCostCenterDto input)
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

        [AbpAuthorize(AppPermissions.Inventory_CostCenters_Create)]
        protected virtual async Task Create(CreateOrEditCostCenterDto input)
        {
            var costCenter = ObjectMapper.Map<CostCenter>(input);


            if (AbpSession.TenantId != null)
            {
                costCenter.TenantId = Convert.ToInt32(AbpSession.TenantId);
            }
            costCenter.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            costCenter.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            costCenter.AudtDate = DateTime.Now;
            costCenter.CreateDate = DateTime.Now;

            await _costCenterRepository.InsertAsync(costCenter);
        }

        [AbpAuthorize(AppPermissions.Inventory_CostCenters_Edit)]
        protected virtual async Task Update(CreateOrEditCostCenterDto input)
        {
            var costCenter = await _costCenterRepository.FirstOrDefaultAsync((int)input.Id);
            input.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            input.AudtDate = DateTime.Now;
            input.CreatedBy = costCenter.CreatedBy;
            input.CreateDate = costCenter.CreateDate;
            ObjectMapper.Map(input, costCenter);
        }

        [AbpAuthorize(AppPermissions.Inventory_CostCenters_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _costCenterRepository.DeleteAsync(input.Id);
        }

        public bool GetcheckCostCenterIdIfExists(string ccId)
        {
            var priceListItem = _costCenterRepository.GetAll().Where(o => o.CCID.ToLower().Trim() == ccId.ToLower().Trim() && o.TenantId == AbpSession.TenantId);

            return priceListItem.Count() == 0 ? false : true;
        }

        public async Task<FileDto> GetDataToExcel(GetAllCostCentersInput input)
        {

            var filteredCostCenters = _costCenterRepository.GetAll()
              .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CCID.Contains(input.Filter) || e.CCName.Contains(input.Filter) || e.AccountID.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
              .WhereIf(!string.IsNullOrWhiteSpace(input.CCIDFilter), e => e.CCID == input.CCIDFilter)
              .WhereIf(!string.IsNullOrWhiteSpace(input.CCNameFilter), e => e.CCName == input.CCNameFilter)
              .WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.AccountID == input.AccountIDFilter)
              .WhereIf(input.MinSubAccIDFilter != null, e => e.SubAccID >= input.MinSubAccIDFilter)
              .WhereIf(input.MaxSubAccIDFilter != null, e => e.SubAccID <= input.MaxSubAccIDFilter)
              .WhereIf(input.MinActiveFilter != null, e => e.Active >= input.MinActiveFilter)
              .WhereIf(input.MaxActiveFilter != null, e => e.Active <= input.MaxActiveFilter)
              .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
              .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
              .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
              .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
              .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
              .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredCostCenters
                         select new GetCostCenterForViewDto()
                         {
                             CostCenter = new CostCenterDto
                             {
                                 CCID = o.CCID,
                                 CCName = o.CCName,
                                 AccountID = o.AccountID,
                                 AccountName = _chartOfControlRepository.GetAll().Where(p => p.Id == o.AccountID && o.TenantId == AbpSession.TenantId).SingleOrDefault().AccountName,
                                 SubAccID = o.SubAccID,
                                 SubAccName = o.SubAccID > 0 ? _accountSubLedgerRepository.GetAll().Where(p => p.AccountID == o.AccountID && p.Id == o.SubAccID && o.TenantId == AbpSession.TenantId).SingleOrDefault().SubAccName : "",
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });


            var dataDto = await query.ToListAsync();

            return _ICostCenterExcelExporter.ExportToFile(dataDto);
        }

    }
}