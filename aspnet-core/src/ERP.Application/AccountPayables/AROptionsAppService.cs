using ERP.CommonServices;
using ERP.GeneralLedger.SetupForms;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.AccountPayables.Exporting;
using ERP.AccountPayables.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.AccountPayables
{
    [AbpAuthorize(AppPermissions.Pages_AROptions)]
    public class AROptionsAppService : ERPAppServiceBase, IAROptionsAppService
    {
		 private readonly IRepository<AROption> _arOptionRepository;
		 private readonly IAROptionsExcelExporter _arOptionsExcelExporter;
		 private readonly IRepository<Bank,int> _lookup_bankRepository;
		 private readonly IRepository<CurrencyRate,string> _lookup_currencyRateRepository;
		 private readonly IRepository<ChartofControl,string> _lookup_chartofControlRepository;
		 

		  public AROptionsAppService(IRepository<AROption> arOptionRepository, IAROptionsExcelExporter arOptionsExcelExporter , IRepository<Bank, int> lookup_bankRepository, IRepository<CurrencyRate, string> lookup_currencyRateRepository, IRepository<ChartofControl, string> lookup_chartofControlRepository) 
		  {
			_arOptionRepository = arOptionRepository;
			_arOptionsExcelExporter = arOptionsExcelExporter;
			_lookup_bankRepository = lookup_bankRepository;
		_lookup_currencyRateRepository = lookup_currencyRateRepository;
		_lookup_chartofControlRepository = lookup_chartofControlRepository;
		
		  }

		 public async Task<PagedResultDto<GetAROptionForViewDto>> GetAll(GetAllAROptionsInput input)
         {

            var filteredAROptions = _arOptionRepository.GetAll()
                        //.Include( e => e.BankFk)
                        //.Include( e => e.CurrencyRateFk)
                        //.Include( e => e.ChartofControlFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DEFBANKID.Contains(input.Filter) || e.DEFCUSCTRLACC.Contains(input.Filter) || e.DEFCURRCODE.Contains(input.Filter) || e.PAYTERMS.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DEFBANKIDFilter), e => e.DEFBANKID.ToLower() == input.DEFBANKIDFilter.ToLower().Trim())
                        .WhereIf(input.MinDEFPAYCODEFilter != null, e => e.DEFPAYCODE >= input.MinDEFPAYCODEFilter)
                        .WhereIf(input.MaxDEFPAYCODEFilter != null, e => e.DEFPAYCODE <= input.MaxDEFPAYCODEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DEFCUSCTRLACCFilter), e => e.DEFCUSCTRLACC.ToLower() == input.DEFCUSCTRLACCFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DEFCURRCODEFilter), e => e.DEFCURRCODE.ToLower() == input.DEFCURRCODEFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PAYTERMSFilter), e => e.PAYTERMS.ToLower() == input.PAYTERMSFilter.ToLower().Trim())
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim()).Where(o => o.TenantId == AbpSession.TenantId);
						//.WhereIf(!string.IsNullOrWhiteSpace(input.BankBANKIDFilter), e => e.BankFk != null && e.BankFk.BANKID.ToLower() == input.BankBANKIDFilter.ToLower().Trim())
						//.WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyRateIdFilter), e => e.CurrencyRateFk != null && e.CurrencyRateFk.Id.ToLower() == input.CurrencyRateIdFilter.ToLower().Trim())
						//.WhereIf(!string.IsNullOrWhiteSpace(input.ChartofControlIdFilter), e => e.ChartofControlFk != null && e.ChartofControlFk.Id.ToLower() == input.ChartofControlIdFilter.ToLower().Trim());

			var pagedAndFilteredAROptions = filteredAROptions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var arOptions = from o in pagedAndFilteredAROptions
                         //join o1 in _lookup_bankRepository.GetAll() on o.BankId equals o1.Id into j1
                         //from s1 in j1.DefaultIfEmpty()
                         
                         //join o2 in _lookup_currencyRateRepository.GetAll() on o.CurrencyRateId equals o2.Id into j2
                         //from s2 in j2.DefaultIfEmpty()
                         
                         //join o3 in _lookup_chartofControlRepository.GetAll() on o.ChartofControlId equals o3.Id into j3
                         //from s3 in j3.DefaultIfEmpty()
                         
                         select new GetAROptionForViewDto() {
							AROption = new AROptionDto
							{
                                DEFBANKID = o.DEFBANKID,
                                DEFPAYCODE = o.DEFPAYCODE,
                                DEFCUSCTRLACC = o.DEFCUSCTRLACC,
                                DEFCURRCODE = o.DEFCURRCODE,
                                PAYTERMS = o.PAYTERMS,
                                AUDTDATE = o.AUDTDATE,
                                AUDTUSER = o.AUDTUSER,
                                Id = o.Id
							},
                         	//BankBANKID = s1 == null ? "" : s1.BANKID.ToString(),
                         	//CurrencyRateId = s2 == null ? "" : s2.Id.ToString(),
                         	//ChartofControlId = s3 == null ? "" : s3.Id.ToString()
						};

            var totalCount = await filteredAROptions.CountAsync();

            return new PagedResultDto<GetAROptionForViewDto>(
                totalCount,
                await arOptions.ToListAsync()
            );
         }
		 
		 public async Task<GetAROptionForViewDto> GetAROptionForView(int id)
         {
            var arOption = await _arOptionRepository.GetAsync(id);

            var output = new GetAROptionForViewDto { AROption = ObjectMapper.Map<AROptionDto>(arOption) };

            if (output.AROption.DEFCURRCODE != null)
            {
                var _lookupCurrencyRate = await _lookup_currencyRateRepository.FirstOrDefaultAsync((string)output.AROption.DEFCURRCODE);
                output.CurrencyRateId = _lookupCurrencyRate.Id.ToString();
            }

            if (output.AROption.DEFBANKID != null)
            {
                var _lookupBank = _lookup_bankRepository.GetAll().Where(o => o.BANKID == output.AROption.DEFBANKID && o.TenantId == AbpSession.TenantId).SingleOrDefault().BANKNAME;
                output.BankBANKID = _lookupBank;
            }

            if (output.AROption.DEFCUSCTRLACC != null)
            {
                var _lookupChartofControl = _lookup_chartofControlRepository.GetAll().Where(o => o.Id == output.AROption.DEFCUSCTRLACC && o.TenantId == AbpSession.TenantId).SingleOrDefault().AccountName;
                output.ChartofControlId = _lookupChartofControl;
            }

            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_AROptions_Edit)]
		 public async Task<GetAROptionForEditOutput> GetAROptionForEdit(EntityDto input)
          {
            var arOption = await _arOptionRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetAROptionForEditOutput {AROption = ObjectMapper.Map<CreateOrEditAROptionDto>(arOption)};

            if (output.AROption.DEFCURRCODE != null)
            {
                var _lookupCurrencyRate = await _lookup_currencyRateRepository.FirstOrDefaultAsync((string)output.AROption.DEFCURRCODE);
                output.CurrencyRateId = _lookupCurrencyRate.Id.ToString();
            }

            if (output.AROption.DEFBANKID != null)
            {
                var _lookupBank = _lookup_bankRepository.GetAll().Where(o => o.BANKID == output.AROption.DEFBANKID && o.TenantId == AbpSession.TenantId).SingleOrDefault().BANKNAME;
                output.BankBANKID = _lookupBank;
            }

            if (output.AROption.DEFCUSCTRLACC != null)
            {
                var _lookupChartofControl = _lookup_chartofControlRepository.GetAll().Where(o => o.Id == output.AROption.DEFCUSCTRLACC && o.TenantId == AbpSession.TenantId).SingleOrDefault().AccountName;
                output.ChartofControlId = _lookupChartofControl;
            }

            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditAROptionDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_AROptions_Create)]
		 protected virtual async Task Create(CreateOrEditAROptionDto input)
         {
            var record = _arOptionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).Count();
            if (record == 0)
            {
                var arOption = ObjectMapper.Map<AROption>(input);
                if (AbpSession.TenantId != null)
                {
                    arOption.TenantId = (int)AbpSession.TenantId;
                }
                await _arOptionRepository.InsertAsync(arOption);
            }
        }

		 [AbpAuthorize(AppPermissions.Pages_AROptions_Edit)]
		 protected virtual async Task Update(CreateOrEditAROptionDto input)
         {
            var arOption = await _arOptionRepository.FirstOrDefaultAsync((int)input.Id);
         
            ObjectMapper.Map(input, arOption);
         }

		 [AbpAuthorize(AppPermissions.Pages_AROptions_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _arOptionRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetAROptionsToExcel(GetAllAROptionsForExcelInput input)
         {

            var filteredAROptions = _arOptionRepository.GetAll()
                        //.Include( e => e.BankFk)
                        //.Include( e => e.CurrencyRateFk)
                        //.Include( e => e.ChartofControlFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DEFBANKID.Contains(input.Filter) || e.DEFCUSCTRLACC.Contains(input.Filter) || e.DEFCURRCODE.Contains(input.Filter) || e.PAYTERMS.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DEFBANKIDFilter), e => e.DEFBANKID.ToLower() == input.DEFBANKIDFilter.ToLower().Trim())
                        .WhereIf(input.MinDEFPAYCODEFilter != null, e => e.DEFPAYCODE >= input.MinDEFPAYCODEFilter)
                        .WhereIf(input.MaxDEFPAYCODEFilter != null, e => e.DEFPAYCODE <= input.MaxDEFPAYCODEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DEFCUSCTRLACCFilter), e => e.DEFCUSCTRLACC.ToLower() == input.DEFCUSCTRLACCFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DEFCURRCODEFilter), e => e.DEFCURRCODE.ToLower() == input.DEFCURRCODEFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PAYTERMSFilter), e => e.PAYTERMS.ToLower() == input.PAYTERMSFilter.ToLower().Trim())
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim()).Where(o => o.TenantId == AbpSession.TenantId);
						//.WhereIf(!string.IsNullOrWhiteSpace(input.BankBANKIDFilter), e => e.BankFk != null && e.BankFk.BANKID.ToLower() == input.BankBANKIDFilter.ToLower().Trim())
						//.WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyRateIdFilter), e => e.CurrencyRateFk != null && e.CurrencyRateFk.Id.ToLower() == input.CurrencyRateIdFilter.ToLower().Trim())
						//.WhereIf(!string.IsNullOrWhiteSpace(input.ChartofControlIdFilter), e => e.ChartofControlFk != null && e.ChartofControlFk.Id.ToLower() == input.ChartofControlIdFilter.ToLower().Trim());

			var query = (from o in filteredAROptions
                         //join o1 in _lookup_bankRepository.GetAll() on o.BankId equals o1.Id into j1
                         //from s1 in j1.DefaultIfEmpty()
                         
                         //join o2 in _lookup_currencyRateRepository.GetAll() on o.CurrencyRateId equals o2.Id into j2
                         //from s2 in j2.DefaultIfEmpty()
                         
                         //join o3 in _lookup_chartofControlRepository.GetAll() on o.ChartofControlId equals o3.Id into j3
                         //from s3 in j3.DefaultIfEmpty()
                         
                         select new GetAROptionForViewDto() { 
							AROption = new AROptionDto
							{
                                DEFBANKID = o.DEFBANKID,
                                DEFPAYCODE = o.DEFPAYCODE,
                                DEFCUSCTRLACC = o.DEFCUSCTRLACC,
                                DEFCURRCODE = o.DEFCURRCODE,
                                PAYTERMS = o.PAYTERMS,
                                AUDTDATE = o.AUDTDATE,
                                AUDTUSER = o.AUDTUSER,
                                Id = o.Id
							},
                         	//BankBANKID = s1 == null ? "" : s1.BANKID.ToString(),
                         	//CurrencyRateId = s2 == null ? "" : s2.Id.ToString(),
                         	//ChartofControlId = s3 == null ? "" : s3.Id.ToString()
						 });


            var arOptionListDtos = await query.ToListAsync();

            return _arOptionsExcelExporter.ExportToFile(arOptionListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_AROptions)]
         public async Task<PagedResultDto<AROptionBankLookupTableDto>> GetAllBankForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_bankRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.BANKID.ToString().Contains(input.Filter)
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var totalCount = await query.CountAsync();

            var bankList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<AROptionBankLookupTableDto>();
			foreach(var bank in bankList){
				lookupTableDtoList.Add(new AROptionBankLookupTableDto
				{
                    Id = bank.BANKID,
                    DisplayName = bank.BANKNAME?.ToString()
                });
			}

            return new PagedResultDto<AROptionBankLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_AROptions)]
         public async Task<PagedResultDto<AROptionCurrencyRateLookupTableDto>> GetAllCurrencyRateForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_currencyRateRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Id.ToString().Contains(input.Filter)
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var totalCount = await query.CountAsync();

            var currencyRateList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<AROptionCurrencyRateLookupTableDto>();
			foreach(var currencyRate in currencyRateList){
				lookupTableDtoList.Add(new AROptionCurrencyRateLookupTableDto
				{
                    Id = currencyRate.Id,
                    DisplayName = currencyRate.Id?.ToString()
                });
			}

            return new PagedResultDto<AROptionCurrencyRateLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_AROptions)]
         public async Task<PagedResultDto<AROptionChartofControlLookupTableDto>> GetAllChartofControlForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_chartofControlRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Id.ToString().Contains(input.Filter)
                ).Where(e => e.SubLedger == true).Where(o => o.TenantId == AbpSession.TenantId);

            var totalCount = await query.CountAsync();

            var chartofControlList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<AROptionChartofControlLookupTableDto>();
			foreach(var chartofControl in chartofControlList){
				lookupTableDtoList.Add(new AROptionChartofControlLookupTableDto
				{
                    Id = chartofControl.Id,
                    DisplayName = chartofControl.AccountName?.ToString()
                });
			}

            return new PagedResultDto<AROptionChartofControlLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}