using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Sales.SaleQutation.Exporting;
using ERP.SupplyChain.Sales.SaleQutation.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;

namespace ERP.SupplyChain.Sales.SaleQutation
{
    [AbpAuthorize(AppPermissions.Pages_OEQD)]
    public class OEQDAppService : ERPAppServiceBase, IOEQDAppService
    {
        private readonly IRepository<OEQD> _oeqdRepository;
        private readonly IOEQDExcelExporter _oeqdExcelExporter;

        public OEQDAppService(IRepository<OEQD> oeqdRepository, IOEQDExcelExporter oeqdExcelExporter)
        {
            _oeqdRepository = oeqdRepository;
            _oeqdExcelExporter = oeqdExcelExporter;

        }

        public async Task<PagedResultDto<GetOEQDForViewDto>> GetAll(GetAllOEQDInput input)
        {

            var filteredOEQD = _oeqdRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ItemID.Contains(input.Filter) || e.Unit.Contains(input.Filter) || e.TaxAuth.Contains(input.Filter) || e.Remarks.Contains(input.Filter))
                        .WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
                        .WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
                        .WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
                        .WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinTransTypeFilter != null, e => e.TransType >= input.MinTransTypeFilter)
                        .WhereIf(input.MaxTransTypeFilter != null, e => e.TransType <= input.MaxTransTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ItemIDFilter), e => e.ItemID == input.ItemIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UnitFilter), e => e.Unit == input.UnitFilter)
                        .WhereIf(input.MinConverFilter != null, e => e.Conver >= input.MinConverFilter)
                        .WhereIf(input.MaxConverFilter != null, e => e.Conver <= input.MaxConverFilter)
                        .WhereIf(input.MinQtyFilter != null, e => e.Qty >= input.MinQtyFilter)
                        .WhereIf(input.MaxQtyFilter != null, e => e.Qty <= input.MaxQtyFilter)
                        .WhereIf(input.MinRateFilter != null, e => e.Rate >= input.MinRateFilter)
                        .WhereIf(input.MaxRateFilter != null, e => e.Rate <= input.MaxRateFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxAuthFilter), e => e.TaxAuth == input.TaxAuthFilter)
                        .WhereIf(input.MinTaxClassFilter != null, e => e.TaxClass >= input.MinTaxClassFilter)
                        .WhereIf(input.MaxTaxClassFilter != null, e => e.TaxClass <= input.MaxTaxClassFilter)
                        .WhereIf(input.MinTaxRateFilter != null, e => e.TaxRate >= input.MinTaxRateFilter)
                        .WhereIf(input.MaxTaxRateFilter != null, e => e.TaxRate <= input.MaxTaxRateFilter)
                        .WhereIf(input.MinTaxAmtFilter != null, e => e.TaxAmt >= input.MinTaxAmtFilter)
                        .WhereIf(input.MaxTaxAmtFilter != null, e => e.TaxAmt <= input.MaxTaxAmtFilter)
                        .WhereIf(input.MinNetAmountFilter != null, e => e.NetAmount >= input.MinNetAmountFilter)
                        .WhereIf(input.MaxNetAmountFilter != null, e => e.NetAmount <= input.MaxNetAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter), e => e.Remarks == input.RemarksFilter);

            var pagedAndFilteredOEQD = filteredOEQD
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var oeqd = from o in pagedAndFilteredOEQD
                       select new
                       {

                           o.DetID,
                           o.LocID,
                           o.DocNo,
                           o.TransType,
                           o.ItemID,
                           o.Unit,
                           o.Conver,
                           o.Qty,
                           o.Rate,
                           o.Amount,
                           o.TaxAuth,
                           o.TaxClass,
                           o.TaxRate,
                           o.TaxAmt,
                           o.NetAmount,
                           o.Remarks,
                           Id = o.Id
                       };

            var totalCount = await filteredOEQD.CountAsync();

            var dbList = await oeqd.ToListAsync();
            var results = new List<GetOEQDForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOEQDForViewDto()
                {
                    OEQD = new OEQDDto
                    {

                        DetID = o.DetID,
                        LocID = o.LocID,
                        DocNo = o.DocNo,
                        TransType = o.TransType,
                        ItemID = o.ItemID,
                        Unit = o.Unit,
                        Conver = o.Conver,
                        Qty = o.Qty,
                        Rate = o.Rate,
                        Amount = o.Amount,
                        TaxAuth = o.TaxAuth,
                        TaxClass = o.TaxClass,
                        TaxRate = o.TaxRate,
                        TaxAmt = o.TaxAmt,
                        NetAmount = o.NetAmount,
                        Remarks = o.Remarks,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOEQDForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOEQDForViewDto> GetOEQDForView(int id)
        {
            var oeqd = await _oeqdRepository.GetAsync(id);

            var output = new GetOEQDForViewDto { OEQD = ObjectMapper.Map<OEQDDto>(oeqd) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_OEQD_Edit)]
        public async Task<GetOEQDForEditOutput> GetOEQDForEdit(EntityDto input)
        {
            var oeqd = await _oeqdRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOEQDForEditOutput { OEQD = ObjectMapper.Map<CreateOrEditOEQDDto>(oeqd) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOEQDDto input)
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

        [AbpAuthorize(AppPermissions.Pages_OEQD_Create)]
        protected virtual async Task Create(CreateOrEditOEQDDto input)
        {
            var oeqd = ObjectMapper.Map<OEQD>(input);

            if (AbpSession.TenantId != null)
            {
                oeqd.TenantId = (int)AbpSession.TenantId;
            }

            await _oeqdRepository.InsertAsync(oeqd);

        }

        [AbpAuthorize(AppPermissions.Pages_OEQD_Edit)]
        protected virtual async Task Update(CreateOrEditOEQDDto input)
        {
            var oeqd = await _oeqdRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, oeqd);

        }

        [AbpAuthorize(AppPermissions.Pages_OEQD_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _oeqdRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetOEQDToExcel(GetAllOEQDForExcelInput input)
        {

            var filteredOEQD = _oeqdRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ItemID.Contains(input.Filter) || e.Unit.Contains(input.Filter) || e.TaxAuth.Contains(input.Filter) || e.Remarks.Contains(input.Filter))
                        .WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
                        .WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
                        .WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
                        .WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinTransTypeFilter != null, e => e.TransType >= input.MinTransTypeFilter)
                        .WhereIf(input.MaxTransTypeFilter != null, e => e.TransType <= input.MaxTransTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ItemIDFilter), e => e.ItemID == input.ItemIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UnitFilter), e => e.Unit == input.UnitFilter)
                        .WhereIf(input.MinConverFilter != null, e => e.Conver >= input.MinConverFilter)
                        .WhereIf(input.MaxConverFilter != null, e => e.Conver <= input.MaxConverFilter)
                        .WhereIf(input.MinQtyFilter != null, e => e.Qty >= input.MinQtyFilter)
                        .WhereIf(input.MaxQtyFilter != null, e => e.Qty <= input.MaxQtyFilter)
                        .WhereIf(input.MinRateFilter != null, e => e.Rate >= input.MinRateFilter)
                        .WhereIf(input.MaxRateFilter != null, e => e.Rate <= input.MaxRateFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxAuthFilter), e => e.TaxAuth == input.TaxAuthFilter)
                        .WhereIf(input.MinTaxClassFilter != null, e => e.TaxClass >= input.MinTaxClassFilter)
                        .WhereIf(input.MaxTaxClassFilter != null, e => e.TaxClass <= input.MaxTaxClassFilter)
                        .WhereIf(input.MinTaxRateFilter != null, e => e.TaxRate >= input.MinTaxRateFilter)
                        .WhereIf(input.MaxTaxRateFilter != null, e => e.TaxRate <= input.MaxTaxRateFilter)
                        .WhereIf(input.MinTaxAmtFilter != null, e => e.TaxAmt >= input.MinTaxAmtFilter)
                        .WhereIf(input.MaxTaxAmtFilter != null, e => e.TaxAmt <= input.MaxTaxAmtFilter)
                        .WhereIf(input.MinNetAmountFilter != null, e => e.NetAmount >= input.MinNetAmountFilter)
                        .WhereIf(input.MaxNetAmountFilter != null, e => e.NetAmount <= input.MaxNetAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter), e => e.Remarks == input.RemarksFilter);

            var query = (from o in filteredOEQD
                         select new GetOEQDForViewDto()
                         {
                             OEQD = new OEQDDto
                             {
                                 DetID = o.DetID,
                                 LocID = o.LocID,
                                 DocNo = o.DocNo,
                                 TransType = o.TransType,
                                 ItemID = o.ItemID,
                                 Unit = o.Unit,
                                 Conver = o.Conver,
                                 Qty = o.Qty,
                                 Rate = o.Rate,
                                 Amount = o.Amount,
                                 TaxAuth = o.TaxAuth,
                                 TaxClass = o.TaxClass,
                                 TaxRate = o.TaxRate,
                                 TaxAmt = o.TaxAmt,
                                 NetAmount = o.NetAmount,
                                 Remarks = o.Remarks,
                                 Id = o.Id
                             }
                         });

            var oeqdListDtos = await query.ToListAsync();

            return _oeqdExcelExporter.ExportToFile(oeqdListDtos);
        }

    }
}