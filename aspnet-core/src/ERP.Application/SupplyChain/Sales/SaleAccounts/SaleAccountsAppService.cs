

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Sales.SaleAccounts.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.Sales.SaleAccounts;
using ERP.SupplyChain.Sales.SaleAccounts.Dtos;
using ERP.SupplyChain.Inventory;
using ERP.Authorization.Users;
using ERP.GeneralLedger.SetupForms;
using ERP.SupplyChain.Purchase.Exporting;

namespace ERP.SupplyChain.Sales.SaleAccounts
{
    [AbpAuthorize(AppPermissions.Sales_SaleAccounts)]
    public class SaleAccountsAppService : ERPAppServiceBase, ISaleAccountsAppService
    {
        private readonly IRepository<OECOLL> _oecollRepository;
        private readonly IRepository<TransactionType> _transactionRepository;
        private readonly IRepository<ICLocation> _locRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly ISaleAccountsExcelExporter _saleAccountsExcelExporter;
        public SaleAccountsAppService(IRepository<OECOLL> oecollRepository,
            IRepository<TransactionType> transactionRepository,
            IRepository<ICLocation> locRepository,
            IRepository<User, long> userRepository,
            IRepository<ChartofControl, string> chartofControlRepository,
            ISaleAccountsExcelExporter saleAccountsExcelExporter
            )
        {
            _oecollRepository = oecollRepository;
            _transactionRepository = transactionRepository;
            _locRepository = locRepository;
            _userRepository = userRepository;
            _chartofControlRepository = chartofControlRepository;
            _saleAccountsExcelExporter = saleAccountsExcelExporter;
        }

        public async Task<PagedResultDto<GetOECOLLForViewDto>> GetAll(GetAllSaleAccountsInput input)
        {
            IQueryable<OECOLLDto> filteredSaleAccounts;
            if (input.Filter == null)
            {
                filteredSaleAccounts = from a in _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                       join
                                       b in _locRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                       on new { A = a.LocID, B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
                                       join
                                       c in _transactionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                       on new { A = a.TypeID, B = a.TenantId } equals new { A = c.TypeId, B = c.TenantId }
                                       select new OECOLLDto()
                                       {
                                           LocID = b.LocID,
                                           LocName = b.LocName,
                                           TypeID = c.TypeId,
                                           TypeName = c.Description,
                                           Id = a.Id
                                       };
            }
            else
            {
                filteredSaleAccounts = from a in _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                       join
                                       b in _locRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                       on new { A = a.LocID, B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
                                       join
                                       c in _transactionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                       on new { A = a.TypeID, B = a.TenantId } equals new { A = c.TypeId, B = c.TenantId }
                                       where (b.LocName.ToLower().Contains(input.Filter.ToLower()) || b.LocID.ToString().ToLower() == input.Filter.ToLower()
                                       || c.Description.ToLower().Contains(input.Filter.ToLower()) || c.TypeId.ToString() == input.Filter.ToLower())
                                       select new OECOLLDto()
                                       {
                                           LocID = b.LocID,
                                           LocName = b.LocName,
                                           TypeID = c.TypeId,
                                           TypeName = c.Description,
                                           Id = a.Id
                                       };
            }


            //var filteredSaleAccounts = _oecollRepository.GetAll()
            //            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TypeID.Contains(input.Filter) || e.SalesACC.Contains(input.Filter) || e.SalesRetACC.Contains(input.Filter) || e.COGSACC.Contains(input.Filter) || e.ChAccountID.Contains(input.Filter) || e.DiscAcc.Contains(input.Filter) || e.WriteOffAcc.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
            //            .WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
            //            .WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
            //            .WhereIf(!string.IsNullOrWhiteSpace(input.TypeIDFilter), e => e.TypeID == input.TypeIDFilter)
            //            .WhereIf(!string.IsNullOrWhiteSpace(input.SalesACCFilter), e => e.SalesACC == input.SalesACCFilter)
            //            .WhereIf(!string.IsNullOrWhiteSpace(input.SalesRetACCFilter), e => e.SalesRetACC == input.SalesRetACCFilter)
            //            .WhereIf(!string.IsNullOrWhiteSpace(input.COGSACCFilter), e => e.COGSACC == input.COGSACCFilter)
            //            .WhereIf(!string.IsNullOrWhiteSpace(input.ChAccountIDFilter), e => e.ChAccountID == input.ChAccountIDFilter)
            //            .WhereIf(!string.IsNullOrWhiteSpace(input.DiscAccFilter), e => e.DiscAcc == input.DiscAccFilter)
            //            .WhereIf(!string.IsNullOrWhiteSpace(input.WriteOffAccFilter), e => e.WriteOffAcc == input.WriteOffAccFilter)
            //            .WhereIf(input.MinActiveFilter != null, e => e.Active >= input.MinActiveFilter)
            //            .WhereIf(input.MaxActiveFilter != null, e => e.Active <= input.MaxActiveFilter)
            //            .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
            //            .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
            //            .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
            //            .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
            //            .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
            //            .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredSaleAccounts = filteredSaleAccounts
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var saleAccounts = from o in pagedAndFilteredSaleAccounts
                               select new GetOECOLLForViewDto()
                               {
                                   OECOLL = new OECOLLDto
                                   {
                                       LocID = o.LocID,
                                       LocName = o.LocName,
                                       TypeID = o.TypeID,
                                       TypeName = o.TypeName,
                                       Id = o.Id
                                   }
                               };

            var totalCount = await filteredSaleAccounts.CountAsync();

            return new PagedResultDto<GetOECOLLForViewDto>(
                totalCount,
                await saleAccounts.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Sales_SaleAccounts_Edit)]
        public async Task<GetOECOLLForEditOutput> GetOECOLLForEdit(EntityDto input)
        {
            var oecoll = await _oecollRepository.FirstOrDefaultAsync(input.Id);
            var output = new GetOECOLLForEditOutput { OECOLL = ObjectMapper.Map<CreateOrEditOECOLLDto>(oecoll) };
            if(!string.IsNullOrEmpty(output.OECOLL.SalesACC))
              output.OECOLL.SalesACCDesc = _chartofControlRepository.GetAll().Where(o => o.Id == output.OECOLL.SalesACC && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName;


            if (!string.IsNullOrEmpty(output.OECOLL.SalesRetACC))
                output.OECOLL.SalesRetACCDesc = _chartofControlRepository.GetAll().Where(o => o.Id == output.OECOLL.SalesRetACC && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName;

            if (!string.IsNullOrEmpty(output.OECOLL.COGSACC))
                output.OECOLL.COGSACCDesc = _chartofControlRepository.GetAll().Where(o => o.Id == output.OECOLL.COGSACC && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName;

            if (!string.IsNullOrEmpty(output.OECOLL.ChAccountID))
                output.OECOLL.ChAccountIDDesc = _chartofControlRepository.GetAll().Where(o => o.Id == output.OECOLL.ChAccountID && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName;

            if (!string.IsNullOrEmpty(output.OECOLL.DiscAcc))
                output.OECOLL.DiscAccDesc = _chartofControlRepository.GetAll().Where(o => o.Id == output.OECOLL.DiscAcc && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName;

            if (!string.IsNullOrEmpty(output.OECOLL.WriteOffAcc))
                output.OECOLL.WriteOffAccDesc = _chartofControlRepository.GetAll().Where(o => o.Id == output.OECOLL.WriteOffAcc && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName;

            if (!string.IsNullOrEmpty(output.OECOLL.SalesACC))
                output.OECOLL.SalesACCDesc = _chartofControlRepository.GetAll().Where(o => o.Id == output.OECOLL.SalesACC && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName;


            if (output.OECOLL.LocID > 0)
                output.OECOLL.LocName = _locRepository.GetAll().Where(o => o.LocID == output.OECOLL.LocID && o.TenantId == AbpSession.TenantId).FirstOrDefault().LocName;

            if (!string.IsNullOrEmpty(output.OECOLL.TypeID))
                output.OECOLL.typeDesc = _transactionRepository.GetAll().Where(o => o.TypeId == output.OECOLL.TypeID && o.TenantId == AbpSession.TenantId).FirstOrDefault().Description;

            if (!string.IsNullOrEmpty(output.OECOLL.RefundableAcc))
                output.OECOLL.RefundableAccDesc = _chartofControlRepository.GetAll().Where(o => o.Id == output.OECOLL.RefundableAcc && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName;

            if (!string.IsNullOrEmpty(output.OECOLL.PayableAcc))
                output.OECOLL.PayableAccDesc = _chartofControlRepository.GetAll().Where(o => o.Id == output.OECOLL.PayableAcc && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName;
            //  var output = new GetOECOLLForEditOutput { OECOLL = ObjectMapper.Map<CreateOrEditOECOLLDto>(oecoll) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOECOLLDto input)
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

        [AbpAuthorize(AppPermissions.Sales_SaleAccounts_Create)]
        protected virtual async Task Create(CreateOrEditOECOLLDto input)
        {
            var oecoll = ObjectMapper.Map<OECOLL>(input);
            oecoll.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            oecoll.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            oecoll.AudtDate = DateTime.Now;
            oecoll.CreateDate = DateTime.Now;
            oecoll.Active = 1;
            if (AbpSession.TenantId != null)
            {
                oecoll.TenantId = (int)AbpSession.TenantId;
            }


            await _oecollRepository.InsertAsync(oecoll);
        }

        [AbpAuthorize(AppPermissions.Sales_SaleAccounts_Edit)]
        protected virtual async Task Update(CreateOrEditOECOLLDto input)
        {
            var oecoll = await _oecollRepository.FirstOrDefaultAsync((int)input.Id);
            input.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            input.AudtDate = DateTime.Now;
            input.CreatedBy = oecoll.CreatedBy;
            input.CreateDate = oecoll.CreateDate;
            input.Active = 1;
            ObjectMapper.Map(input, oecoll);
        }

        [AbpAuthorize(AppPermissions.Sales_SaleAccounts_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _oecollRepository.DeleteAsync(input.Id);
        }

        public bool GetTransAgainstLoc(int locId, string typeId)
        {
            var data = _oecollRepository.GetAll().Where(o => o.LocID == locId && o.TypeID == typeId && o.TenantId == AbpSession.TenantId);
            return data.Count() > 0 ? true : false;
        }

        public async Task<FileDto> GetDataToExcel(GetAllSaleAccountsInput input)
        {
            IQueryable<OECOLLDto> filteredSaleAccounts;
            if (input.Filter == null)
            {
                filteredSaleAccounts = from a in _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                       join
                                       b in _locRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                       on new { A = a.LocID, B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
                                       join
                                       c in _transactionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                       on new { A = a.TypeID, B = a.TenantId } equals new { A = c.TypeId, B = c.TenantId }
                                       select new OECOLLDto()
                                       {
                                           LocID = b.LocID,
                                           LocName = b.LocName,
                                           TypeID = c.TypeId,
                                           TypeName = c.Description,
                                           Id = a.Id
                                       };
            }
            else
            {
                filteredSaleAccounts = from a in _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                       join
                                       b in _locRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                       on new { A = a.LocID, B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
                                       join
                                       c in _transactionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                       on new { A = a.TypeID, B = a.TenantId } equals new { A = c.TypeId, B = c.TenantId }
                                       where (b.LocName.ToLower().Contains(input.Filter.ToLower()) || b.LocID.ToString().ToLower() == input.Filter.ToLower()
                                       || c.Description.ToLower().Contains(input.Filter.ToLower()) || c.TypeId.ToString() == input.Filter.ToLower())
                                       select new OECOLLDto()
                                       {
                                           LocID = b.LocID,
                                           LocName = b.LocName,
                                           TypeID = c.TypeId,
                                           TypeName = c.Description,
                                           Id = a.Id
                                       };
            }



            var pagedAndFiltered = filteredSaleAccounts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var requisitions = from o in pagedAndFiltered
                               select new GetOECOLLForViewDto()
                               {
                                   OECOLL = new OECOLLDto
                                   {
                                       LocID = o.LocID,
                                       LocName = o.LocName,
                                       TypeID = o.TypeID,
                                       TypeName = o.TypeName,
                                       Id = o.Id
                                   }
                               };

            var dataDto = await requisitions.ToListAsync();

            return _saleAccountsExcelExporter.ExportToFile(dataDto);
        }
    }
}