using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Sales.OECSD.Exporting;
using ERP.SupplyChain.Sales.OECSD.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;

namespace ERP.SupplyChain.Sales.OECSD
{
    [AbpAuthorize(AppPermissions.Pages_OECSD)]
    public class OECSDAppService : ERPAppServiceBase, IOECSDAppService
    {
        private readonly IRepository<OECSD> _oecsdRepository;
        private readonly IOECSDExcelExporter _oecsdExcelExporter;

        public OECSDAppService(IRepository<OECSD> oecsdRepository, IOECSDExcelExporter oecsdExcelExporter)
        {
            _oecsdRepository = oecsdRepository;
            _oecsdExcelExporter = oecsdExcelExporter;

        }

        public async Task<PagedResultDto<GetOECSDForViewDto>> GetAll(GetAllOECSDInput input)
        {

            var filteredOECSD = _oecsdRepository.GetAll()
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

            var pagedAndFilteredOECSD = filteredOECSD
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var oecsd = from o in pagedAndFilteredOECSD
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

            var totalCount = await filteredOECSD.CountAsync();

            var dbList = await oecsd.ToListAsync();
            var results = new List<GetOECSDForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOECSDForViewDto()
                {
                    OECSD = new OECSDDto
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

            return new PagedResultDto<GetOECSDForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOECSDForViewDto> GetOECSDForView(int id)
        {
            var oecsd = await _oecsdRepository.GetAsync(id);

            var output = new GetOECSDForViewDto { OECSD = ObjectMapper.Map<OECSDDto>(oecsd) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_OECSD_Edit)]
        public async Task<GetOECSDForEditOutput> GetOECSDForEdit(EntityDto input)
        {
            var oecsd = await _oecsdRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOECSDForEditOutput { OECSD = ObjectMapper.Map<CreateOrEditOECSDDto>(oecsd) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOECSDDto input)
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

        [AbpAuthorize(AppPermissions.Pages_OECSD_Create)]
        protected virtual async Task Create(CreateOrEditOECSDDto input)
        {
            var oecsd = ObjectMapper.Map<OECSD>(input);

            if (AbpSession.TenantId != null)
            {
                oecsd.TenantId = (int)AbpSession.TenantId;
            }

            await _oecsdRepository.InsertAsync(oecsd);

        }

        [AbpAuthorize(AppPermissions.Pages_OECSD_Edit)]
        protected virtual async Task Update(CreateOrEditOECSDDto input)
        {
            var oecsd = await _oecsdRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, oecsd);

        }

        [AbpAuthorize(AppPermissions.Pages_OECSD_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _oecsdRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetOECSDToExcel(GetAllOECSDForExcelInput input)
        {

            var filteredOECSD = _oecsdRepository.GetAll()
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

            var query = (from o in filteredOECSD
                         select new GetOECSDForViewDto()
                         {
                             OECSD = new OECSDDto
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

            var oecsdListDtos = await query.ToListAsync();

            return _oecsdExcelExporter.ExportToFile(oecsdListDtos);
        }

    }
}