

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory.Exporting;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.Collections.Extensions;
using ERP.AccountPayables.Setup.ApSetup.Dtos;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ERP.SupplyChain.Inventory
{
    [AbpAuthorize(AppPermissions.Inventory_ICSetups)]
    public class ICSetupsAppService : ERPAppServiceBase, IICSetupsAppService
    {
        private readonly IRepository<ICSetup> _icSetupRepository;
        private readonly IRepository<ICLocation> _icLocationRepository;
        private readonly IICSetupsExcelExporter _icSetupsExcelExporter;


        public ICSetupsAppService(IRepository<ICSetup> icSetupRepository, IRepository<ICLocation> icLocationRepository, IICSetupsExcelExporter icSetupsExcelExporter)
        {
            _icSetupRepository = icSetupRepository;
            _icSetupsExcelExporter = icSetupsExcelExporter;
            _icLocationRepository = icLocationRepository;

        }

        public async Task<PagedResultDto<ICSetupDto>> GetAll(GetAllICSetupsInput input)
        {

            var filteredICSetups = _icSetupRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Segment1.Contains(input.Filter) || e.Segment2.Contains(input.Filter) || e.Segment3.Contains(input.Filter) || e.PRBookID.Contains(input.Filter) || e.RTBookID.Contains(input.Filter) || e.CnsBookID.Contains(input.Filter) || e.SLBookID.Contains(input.Filter) || e.SRBookID.Contains(input.Filter) || e.TRBookID.Contains(input.Filter) || e.PrdBookID.Contains(input.Filter) || e.PyRecBookID.Contains(input.Filter) || e.AdjBookID.Contains(input.Filter) || e.AsmBookID.Contains(input.Filter) || e.WSBookID.Contains(input.Filter) || e.DSBookID.Contains(input.Filter) || e.Opt4.Contains(input.Filter) || e.Opt5.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Segment1Filter), e => e.Segment1.ToLower() == input.Segment1Filter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Segment2Filter), e => e.Segment2.ToLower() == input.Segment2Filter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Segment3Filter), e => e.Segment3.ToLower() == input.Segment3Filter.ToLower().Trim());

            var pagedAndFilteredICSetups = filteredICSetups
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var icSetups = from o in pagedAndFilteredICSetups
                           //join l in _icLocationRepository.GetAll() 
                           //on new { a=o.TenantId,b=Convert.ToInt32(o.CurrentLocID)} equals new {a=l.TenantId,b=l.LocID}
                           select new ICSetupDto()
                           {
                               Segment1 = o.Segment1,
                               Segment2 = o.Segment2,
                               Segment3 = o.Segment3,
                               AllowNegative = o.AllowNegative,
                               ErrSrNo = o.ErrSrNo,
                               CostingMethod = o.CostingMethod,
                               PRBookID = o.PRBookID,
                               RTBookID = o.RTBookID,
                               CnsBookID = o.CnsBookID,
                               SLBookID = o.SLBookID,
                               SRBookID = o.SRBookID,
                               TRBookID = o.TRBookID,
                               PrdBookID = o.PrdBookID,
                               PyRecBookID = o.PyRecBookID,
                               AdjBookID = o.AdjBookID,
                               AsmBookID = o.AsmBookID,
                               WSBookID = o.WSBookID,
                               DSBookID = o.DSBookID,
                               SalesReturnLinkOn = o.SalesReturnLinkOn,
                               SalesLinkOn = o.SalesLinkOn,
                               AccLinkOn = o.AccLinkOn,
                               CurrentLocID = o.CurrentLocID,
                               GLSegLink = o.GLSegLink,
                               AllowLocID = o.AllowLocID,
                               CDateOnly = o.CDateOnly,
                               Opt4 = o.Opt4,
                               Opt5 = o.Opt5,
                               CreatedBy = o.CreatedBy,
                               CreateadOn = o.CreateadOn,
                               Id = o.Id,
                               DamageLocID = o.DamageLocID,
                               TransType = o.TransType,
                               CurrentLocName = "",
                               conType = o.conType//l.LocName
                           };

            var totalCount = await filteredICSetups.CountAsync();
            var setupList = await icSetups.ToListAsync();
            return new PagedResultDto<ICSetupDto>(
                totalCount,
               setupList
            );
        }

        public async Task<ICSetupDto> GetICSetupForView(int id)
        {
            var icSetup = await _icSetupRepository.GetAsync(id);

            var output = ObjectMapper.Map<ICSetupDto>(icSetup);

            return output;
        }

        [AbpAuthorize(AppPermissions.Inventory_ICSetups_Edit)]
        public async Task<ICSetupDto> GetICSetupForEdit(EntityDto input)
        {
            var icSetup = await _icSetupRepository.FirstOrDefaultAsync(input.Id);

            var output = ObjectMapper.Map<ICSetupDto>(icSetup);

            return output;
        }


        public async Task CreateOrEdit(ICSetupDto input)
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

        [AbpAuthorize(AppPermissions.Inventory_ICSetups_Create)]
        protected virtual async Task Create(ICSetupDto input)
        {
            var icSetup = ObjectMapper.Map<ICSetup>(input);


            if (AbpSession.TenantId != null)
            {
                icSetup.TenantId = (int)AbpSession.TenantId;
            }


            await _icSetupRepository.InsertAsync(icSetup);
        }

        [AbpAuthorize(AppPermissions.Inventory_ICSetups_Edit)]
        protected virtual async Task Update(ICSetupDto input)
        {
            var icSetup = await _icSetupRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, icSetup);
        }

        [AbpAuthorize(AppPermissions.Inventory_ICSetups_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _icSetupRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetICSetupsToExcel(GetAllICSetupsInput input)
        {

            var filteredICSetups = _icSetupRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Segment1.Contains(input.Filter) || e.Segment2.Contains(input.Filter) || e.Segment3.Contains(input.Filter) || e.PRBookID.Contains(input.Filter) || e.RTBookID.Contains(input.Filter) || e.CnsBookID.Contains(input.Filter) || e.SLBookID.Contains(input.Filter) || e.SRBookID.Contains(input.Filter) || e.TRBookID.Contains(input.Filter) || e.PrdBookID.Contains(input.Filter) || e.PyRecBookID.Contains(input.Filter) || e.AdjBookID.Contains(input.Filter) || e.AsmBookID.Contains(input.Filter) || e.WSBookID.Contains(input.Filter) || e.DSBookID.Contains(input.Filter) || e.Opt4.Contains(input.Filter) || e.Opt5.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Segment1Filter), e => e.Segment1.ToLower() == input.Segment1Filter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Segment2Filter), e => e.Segment2.ToLower() == input.Segment2Filter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Segment3Filter), e => e.Segment3.ToLower() == input.Segment3Filter.ToLower().Trim())
                        .WhereIf(input.MinErrSrNoFilter != null, e => e.ErrSrNo >= input.MinErrSrNoFilter)
                        .WhereIf(input.MaxErrSrNoFilter != null, e => e.ErrSrNo <= input.MaxErrSrNoFilter)
                        .WhereIf(input.MinCostingMethodFilter != null, e => e.CostingMethod >= input.MinCostingMethodFilter)
                        .WhereIf(input.MaxCostingMethodFilter != null, e => e.CostingMethod <= input.MaxCostingMethodFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PRBookIDFilter), e => e.PRBookID.ToLower() == input.PRBookIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RTBookIDFilter), e => e.RTBookID.ToLower() == input.RTBookIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CnsBookIDFilter), e => e.CnsBookID.ToLower() == input.CnsBookIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SLBookIDFilter), e => e.SLBookID.ToLower() == input.SLBookIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SRBookIDFilter), e => e.SRBookID.ToLower() == input.SRBookIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TRBookIDFilter), e => e.TRBookID.ToLower() == input.TRBookIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PrdBookIDFilter), e => e.PrdBookID.ToLower() == input.PrdBookIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PyRecBookIDFilter), e => e.PyRecBookID.ToLower() == input.PyRecBookIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdjBookIDFilter), e => e.AdjBookID.ToLower() == input.AdjBookIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AsmBookIDFilter), e => e.AsmBookID.ToLower() == input.AsmBookIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WSBookIDFilter), e => e.WSBookID.ToLower() == input.WSBookIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DSBookIDFilter), e => e.DSBookID.ToLower() == input.DSBookIDFilter.ToLower().Trim())
                        .WhereIf(input.MinCurrentLocIDFilter != null, e => e.CurrentLocID >= input.MinCurrentLocIDFilter)
                        .WhereIf(input.MaxCurrentLocIDFilter != null, e => e.CurrentLocID <= input.MaxCurrentLocIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Opt4Filter), e => e.Opt4.ToLower() == input.Opt4Filter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Opt5Filter), e => e.Opt5.ToLower() == input.Opt5Filter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                        .WhereIf(input.MinCreateadOnFilter != null, e => e.CreateadOn >= input.MinCreateadOnFilter)
                        .WhereIf(input.MaxCreateadOnFilter != null, e => e.CreateadOn <= input.MaxCreateadOnFilter);

            var query = (from o in filteredICSetups
                         select new ICSetupDto()
                         {
                             Segment1 = o.Segment1,
                             Segment2 = o.Segment2,
                             Segment3 = o.Segment3,
                             AllowNegative = o.AllowNegative,
                             ErrSrNo = o.ErrSrNo,
                             CostingMethod = o.CostingMethod,
                             PRBookID = o.PRBookID,
                             RTBookID = o.RTBookID,
                             CnsBookID = o.CnsBookID,
                             SLBookID = o.SLBookID,
                             SRBookID = o.SRBookID,
                             TRBookID = o.TRBookID,
                             PrdBookID = o.PrdBookID,
                             PyRecBookID = o.PyRecBookID,
                             AdjBookID = o.AdjBookID,
                             AsmBookID = o.AsmBookID,
                             WSBookID = o.WSBookID,
                             DSBookID = o.DSBookID,
                             SalesReturnLinkOn = o.SalesReturnLinkOn,
                             SalesLinkOn = o.SalesLinkOn,
                             AccLinkOn = o.AccLinkOn,
                             CurrentLocID = o.CurrentLocID,
                             GLSegLink = o.GLSegLink,
                             AllowLocID = o.AllowLocID,
                             CDateOnly = o.CDateOnly,
                             Opt4 = o.Opt4,
                             Opt5 = o.Opt5,
                             CreatedBy = o.CreatedBy,
                             CreateadOn = o.CreateadOn,
                             Id = o.Id,
                             DamageLocID = o.DamageLocID,
                             TransType = o.TransType,
                             conType = o.conType
                         });


            var icSetupListDtos = await query.ToListAsync();

            return _icSetupsExcelExporter.ExportToFile(icSetupListDtos);
        }

        public ApSetupDto GetDecimalPoints()
        {
            int tenantId = (int)AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            ApSetupDto model = new ApSetupDto();
            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetDecimalPlaces", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TenantID", tenantId);
                    cn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            model.InventoryPoint = Convert.ToInt32(rdr["InventoryPoint"]);
                            model.FinancePoint = Convert.ToInt32(rdr["FinancePoint"]);
                        }
                    }
                }
            }
            return model;
        }


    }
}