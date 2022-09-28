using ERP.GeneralLedger.SetupForms;
using ERP.CommonServices;


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
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.AccountPayables
{
	[AbpAuthorize(AppPermissions.Pages_APOptions)]
    public class APOptionsAppService : ERPAppServiceBase, IAPOptionsAppService
    {
		 private readonly IRepository<APOption> _apOptionRepository;
		 private readonly IAPOptionsExcelExporter _apOptionsExcelExporter;
		 private readonly IRepository<CurrencyRate,string> _lookup_currencyRateRepository;
		 private readonly IRepository<Bank,int> _lookup_bankRepository;
		 private readonly IRepository<ChartofControl,string> _lookup_chartofControlRepository;
        private readonly IRepository<CompanyProfile, string> _companySetupRepository;

        public APOptionsAppService(IRepository<APOption> apOptionRepository, IAPOptionsExcelExporter apOptionsExcelExporter , IRepository<CurrencyRate, string> lookup_currencyRateRepository, IRepository<Bank, int> lookup_bankRepository, IRepository<ChartofControl, string> lookup_chartofControlRepository, IRepository<CompanyProfile, string> companySetupRepository) 
		  {
			_apOptionRepository = apOptionRepository;
			_apOptionsExcelExporter = apOptionsExcelExporter;
			_lookup_currencyRateRepository = lookup_currencyRateRepository;
		_lookup_bankRepository = lookup_bankRepository;
		_lookup_chartofControlRepository = lookup_chartofControlRepository;
            _companySetupRepository = companySetupRepository;
		  }

		 public async Task<PagedResultDto<GetAPOptionForViewDto>> GetAll(GetAllAPOptionsInput input)
         {

            var filteredAPOptions = _apOptionRepository.GetAll()
                        //.Include( e => e.CurrencyRateFk)
                        //.Include( e => e.BankFk)
                        //.Include( e => e.ChartofControlFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DEFBANKID.Contains(input.Filter) || e.DEFVENCTRLACC.Contains(input.Filter) || e.DEFCURRCODE.Contains(input.Filter) || e.PAYTERMS.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DEFBANKIDFilter), e => e.DEFBANKID.ToLower() == input.DEFBANKIDFilter.ToLower().Trim())
                        .WhereIf(input.MinDEFPAYCODEFilter != null, e => e.DEFPAYCODE >= input.MinDEFPAYCODEFilter)
                        .WhereIf(input.MaxDEFPAYCODEFilter != null, e => e.DEFPAYCODE <= input.MaxDEFPAYCODEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DEFVENCTRLACCFilter), e => e.DEFVENCTRLACC.ToLower() == input.DEFVENCTRLACCFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DEFCURRCODEFilter), e => e.DEFCURRCODE.ToLower() == input.DEFCURRCODEFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PAYTERMSFilter), e => e.PAYTERMS.ToLower() == input.PAYTERMSFilter.ToLower().Trim())
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim()).Where(o => o.TenantId == AbpSession.TenantId);
						//.WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyRateIdFilter), e => e.CurrencyRateFk != null && e.CurrencyRateFk.Id.ToLower() == input.CurrencyRateIdFilter.ToLower().Trim())
						//.WhereIf(!string.IsNullOrWhiteSpace(input.BankBANKIDFilter), e => e.BankFk != null && e.BankFk.BANKID.ToLower() == input.BankBANKIDFilter.ToLower().Trim())
						//.WhereIf(!string.IsNullOrWhiteSpace(input.ChartofControlIdFilter), e => e.ChartofControlFk != null && e.ChartofControlFk.Id.ToLower() == input.ChartofControlIdFilter.ToLower().Trim());

			var pagedAndFilteredAPOptions = filteredAPOptions
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

			var apOptions = from o in pagedAndFilteredAPOptions
                         //join o1 in _lookup_currencyRateRepository.GetAll() on o.CurrencyRateId equals o1.Id into j1
                         //from s1 in j1.DefaultIfEmpty()
                         
                         //join o2 in _lookup_bankRepository.GetAll() on o.BankId equals o2.Id into j2
                         //from s2 in j2.DefaultIfEmpty()
                         
                         //join o3 in _lookup_chartofControlRepository.GetAll() on o.ChartofControlId equals o3.Id into j3
                         //from s3 in j3.DefaultIfEmpty()
                         
                         select new GetAPOptionForViewDto() {
							APOption = new APOptionDto
							{
                                DEFBANKID = o.DEFBANKID,
                                DEFPAYCODE = o.DEFPAYCODE,
                                DEFVENCTRLACC = o.DEFVENCTRLACC,
                                DEFCURRCODE = o.DEFCURRCODE,
                                PAYTERMS = o.PAYTERMS,
                                AUDTDATE = o.AUDTDATE,
                                AUDTUSER = o.AUDTUSER,
                                Id = o.Id
							},
                         	//CurrencyRateId = s1 == null ? "" : s1.Id.ToString(),
                         	//BankBANKID = s2 == null ? "" : s2.BANKID.ToString(),
                         	//ChartofControlId = s3 == null ? "" : s3.Id.ToString()
						};

            var totalCount = await filteredAPOptions.CountAsync();

            return new PagedResultDto<GetAPOptionForViewDto>(
                totalCount,
                await apOptions.ToListAsync()
            );
         }
		 

		 public async Task<GetAPOptionForViewDto> GetAPOptionForView(int id)
         {
            var apOption = await _apOptionRepository.GetAsync(id);

            var output = new GetAPOptionForViewDto { APOption = ObjectMapper.Map<APOptionDto>(apOption) };

            if (output.APOption.DEFCURRCODE != null)
            {
                var _lookupCurrencyRate = await _lookup_currencyRateRepository.FirstOrDefaultAsync((string)output.APOption.DEFCURRCODE);
                output.CurrencyRateId = _lookupCurrencyRate.Id.ToString();
            }

            if (output.APOption.DEFBANKID != null)
            {
                var _lookupBank = _lookup_bankRepository.GetAll().Where(o => o.BANKID == output.APOption.DEFBANKID && o.TenantId == AbpSession.TenantId).SingleOrDefault().BANKNAME;
                output.BankBANKID = _lookupBank;
            }

            if (output.APOption.DEFVENCTRLACC != null)
            {
                var _lookupChartofControl = _lookup_chartofControlRepository.GetAll().Where(o => o.Id == output.APOption.DEFVENCTRLACC && o.TenantId == AbpSession.TenantId).SingleOrDefault().AccountName;
                output.ChartofControlId = _lookupChartofControl;
            }

            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_APOptions_Edit)]
		 public async Task<GetAPOptionForEditOutput> GetAPOptionForEdit(EntityDto input)
         {
            var apOption = await _apOptionRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetAPOptionForEditOutput {APOption = ObjectMapper.Map<CreateOrEditAPOptionDto>(apOption)};

            if (output.APOption.DEFCURRCODE != null)
            {
                var _lookupCurrencyRate = await _lookup_currencyRateRepository.FirstOrDefaultAsync((string)output.APOption.DEFCURRCODE);
                output.CurrencyRateId = _lookupCurrencyRate.Id.ToString();
            }

            if (output.APOption.DEFBANKID != null)
            {
                var _lookupBank = _lookup_bankRepository.GetAll().Where(o=>o.BANKID== output.APOption.DEFBANKID && o.TenantId==AbpSession.TenantId).SingleOrDefault().BANKNAME;
                output.BankBANKID = _lookupBank;
            }

            if (output.APOption.DEFVENCTRLACC != null)
            {
                var _lookupChartofControl = _lookup_chartofControlRepository.GetAll().Where(o => o.Id == output.APOption.DEFVENCTRLACC && o.TenantId == AbpSession.TenantId).SingleOrDefault().AccountName;
                output.ChartofControlId = _lookupChartofControl;
            }

            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditAPOptionDto input)
         {
            if (input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_APOptions_Create)]
		 protected virtual async Task Create(CreateOrEditAPOptionDto input)
         {
            var record = _apOptionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).Count();
            if (record == 0)
            {
                var apOption = ObjectMapper.Map<APOption>(input);
                if (AbpSession.TenantId != null)
                {
                    apOption.TenantId = (int)AbpSession.TenantId;
                }
                await _apOptionRepository.InsertAsync(apOption);
            }
         }

		 [AbpAuthorize(AppPermissions.Pages_APOptions_Edit)]
		 protected virtual async Task Update(CreateOrEditAPOptionDto input)
         {
            var apOption = await _apOptionRepository.FirstOrDefaultAsync((int)input.Id);
        
            ObjectMapper.Map(input, apOption);
         }

		 [AbpAuthorize(AppPermissions.Pages_APOptions_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _apOptionRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetAPOptionsToExcel(GetAllAPOptionsForExcelInput input)
         {

            var filteredAPOptions = _apOptionRepository.GetAll()
                        //.Include( e => e.CurrencyRateFk)
                        //.Include( e => e.BankFk)
                        //.Include( e => e.ChartofControlFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DEFBANKID.Contains(input.Filter) || e.DEFVENCTRLACC.Contains(input.Filter) || e.DEFCURRCODE.Contains(input.Filter) || e.PAYTERMS.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DEFBANKIDFilter), e => e.DEFBANKID.ToLower() == input.DEFBANKIDFilter.ToLower().Trim())
                        .WhereIf(input.MinDEFPAYCODEFilter != null, e => e.DEFPAYCODE >= input.MinDEFPAYCODEFilter)
                        .WhereIf(input.MaxDEFPAYCODEFilter != null, e => e.DEFPAYCODE <= input.MaxDEFPAYCODEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DEFVENCTRLACCFilter), e => e.DEFVENCTRLACC.ToLower() == input.DEFVENCTRLACCFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DEFCURRCODEFilter), e => e.DEFCURRCODE.ToLower() == input.DEFCURRCODEFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PAYTERMSFilter), e => e.PAYTERMS.ToLower() == input.PAYTERMSFilter.ToLower().Trim())
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim()).Where(o => o.TenantId == AbpSession.TenantId);
						//.WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyRateIdFilter), e => e.CurrencyRateFk != null && e.CurrencyRateFk.Id.ToLower() == input.CurrencyRateIdFilter.ToLower().Trim())
						//.WhereIf(!string.IsNullOrWhiteSpace(input.BankBANKIDFilter), e => e.BankFk != null && e.BankFk.BANKID.ToLower() == input.BankBANKIDFilter.ToLower().Trim())
						//.WhereIf(!string.IsNullOrWhiteSpace(input.ChartofControlIdFilter), e => e.ChartofControlFk != null && e.ChartofControlFk.Id.ToLower() == input.ChartofControlIdFilter.ToLower().Trim());

			var query = (from o in filteredAPOptions
                         //join o1 in _lookup_currencyRateRepository.GetAll() on o.CurrencyRateId equals o1.Id into j1
                         //from s1 in j1.DefaultIfEmpty()
                         
                         //join o2 in _lookup_bankRepository.GetAll() on o.BankId equals o2.Id into j2
                         //from s2 in j2.DefaultIfEmpty()
                         
                         //join o3 in _lookup_chartofControlRepository.GetAll() on o.ChartofControlId equals o3.Id into j3
                         //from s3 in j3.DefaultIfEmpty()
                         
                         select new GetAPOptionForViewDto() { 
							APOption = new APOptionDto
							{
                                DEFBANKID = o.DEFBANKID,
                                DEFPAYCODE = o.DEFPAYCODE,
                                DEFVENCTRLACC = o.DEFVENCTRLACC,
                                DEFCURRCODE = o.DEFCURRCODE,
                                PAYTERMS = o.PAYTERMS,
                                AUDTDATE = o.AUDTDATE,
                                AUDTUSER = o.AUDTUSER,
                                Id = o.Id
							},
                         	//CurrencyRateId = s1 == null ? "" : s1.Id.ToString(),
                         	//BankBANKID = s2 == null ? "" : s2.BANKID.ToString(),
                         	//ChartofControlId = s3 == null ? "" : s3.Id.ToString()
						 });


            var apOptionListDtos = await query.ToListAsync();

            return _apOptionsExcelExporter.ExportToFile(apOptionListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_APOptions)]
         public async Task<PagedResultDto<APOptionCurrencyRateLookupTableDto>> GetAllCurrencyRateForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_currencyRateRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Id.ToString().Contains(input.Filter)
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var totalCount = await query.CountAsync();

            var currencyRateList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<APOptionCurrencyRateLookupTableDto>();
			foreach(var currencyRate in currencyRateList){
                lookupTableDtoList.Add(new APOptionCurrencyRateLookupTableDto
                {
                    Id = currencyRate.Id,
                    DisplayName = currencyRate.Id,
                    CurrRate=currencyRate.CURRATE
				});
			}

            return new PagedResultDto<APOptionCurrencyRateLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_APOptions)]
         public async Task<PagedResultDto<APOptionBankLookupTableDto>> GetAllBankForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_bankRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.BANKID.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.BANKNAME.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.IDACCTBANK.ToString().ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var totalCount = await query.CountAsync();

            var bankList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<APOptionBankLookupTableDto>();
			foreach(var bank in bankList){
				lookupTableDtoList.Add(new APOptionBankLookupTableDto
				{
					Id = bank.BANKID,
					DisplayName = bank.BANKNAME?.ToString(),
                    AccountID=bank.IDACCTBANK
				});
			}

            return new PagedResultDto<APOptionBankLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_APOptions)]
         public async Task<PagedResultDto<APOptionChartofControlLookupTableDto>> GetAllChartofControlForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_chartofControlRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Id.ToString().Contains(input.Filter)
                ).Where(e => e.SubLedger == true).Where(o => o.TenantId == AbpSession.TenantId);

            var totalCount = await query.CountAsync();

            var chartofControlList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<APOptionChartofControlLookupTableDto>();
			foreach(var chartofControl in chartofControlList){
				lookupTableDtoList.Add(new APOptionChartofControlLookupTableDto
				{
					Id = chartofControl.Id,
					DisplayName = chartofControl.AccountName?.ToString()
				});
			}

            return new PagedResultDto<APOptionChartofControlLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

        public CompanyProfileViewDto GetCompanyProfileData()
        {
            CompanyProfileViewDto getcompanyobjec = new CompanyProfileViewDto();
            var getobject = _companySetupRepository.GetAll().Where(o => o.TenantId == (int)AbpSession.TenantId).FirstOrDefault(); 
            if (getobject != null)
            {
                getcompanyobjec.CONTPERSON = getobject.CONTPERSON;
                getcompanyobjec.CONTPHONE = getobject.CONTPHONE;
                getcompanyobjec.CompanyName = getobject.CompanyName;
                getcompanyobjec.Address = getobject.Address1;
                getcompanyobjec.City = getobject.City;
                getcompanyobjec.State = getobject.State;
                getcompanyobjec.Country = getobject.Country;
                getcompanyobjec.ZipCode = getobject.ZipCode;
            }
            else
            {
                getcompanyobjec.CONTPERSON = "";
                getcompanyobjec.CONTPHONE = "";
                getcompanyobjec.CompanyName = "";
                getcompanyobjec.Address = "";
                getcompanyobjec.City = "";
                getcompanyobjec.State = "";
                getcompanyobjec.Country = "";
                getcompanyobjec.ZipCode = "";

            }
             return getcompanyobjec;

        }
    }
}