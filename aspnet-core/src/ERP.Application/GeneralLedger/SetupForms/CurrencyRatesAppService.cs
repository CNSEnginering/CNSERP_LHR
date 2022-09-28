using ERP.CommonServices;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms.Exporting;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.GeneralLedger.SetupForms
{
    [AbpAuthorize(AppPermissions.SetupForms_CurrencyRates)]
    public class CurrencyRatesAppService : ERPAppServiceBase, ICurrencyRatesAppService
    {
        private readonly IRepository<CurrencyRate, string> _currencyRateRepository;
        private readonly ICurrencyRatesExcelExporter _currencyRatesExcelExporter;
        private readonly IRepository<CompanyProfile, string> _lookup_companyProfileRepository;
        private readonly IRepository<CurrencyRateHistory> _currencyHistoryRepository;

        public CurrencyRatesAppService(IRepository<CurrencyRate, string> currencyRateRepository,
            ICurrencyRatesExcelExporter currencyRatesExcelExporter, IRepository<CompanyProfile, string> lookup_companyProfileRepository,
             IRepository<CurrencyRateHistory> currencyHistoryRepository
            )
        {
            _currencyRateRepository = currencyRateRepository;
            _currencyRatesExcelExporter = currencyRatesExcelExporter;
            _lookup_companyProfileRepository = lookup_companyProfileRepository;
            _currencyHistoryRepository = currencyHistoryRepository;


        }

        public async Task<PagedResultDto<GetCurrencyRateForViewDto>> GetAll(GetAllCurrencyRatesInput input)
        {

            var filteredCurrencyRates = _currencyRateRepository.GetAll()

                        //.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CMPID.Contains(input.Filter) || e.Id.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter) || e.CURNAME.Contains(input.Filter) || e.SYMBOL.Contains(input.Filter))
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.CMPIDFilter), e => e.CMPID.ToLower() == input.CMPIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CURIDFilter), e => e.Id.ToLower() == input.CURIDFilter.ToLower().Trim())
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CURNAMEFilter), e => e.CURNAME.ToLower() == input.CURNAMEFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SYMBOLFilter), e => e.SYMBOL.ToLower() == input.SYMBOLFilter.ToLower().Trim())
                        .WhereIf(input.MinRATEDATEFilter != null, e => e.RATEDATE >= input.MinRATEDATEFilter)
                        .WhereIf(input.MaxRATEDATEFilter != null, e => e.RATEDATE <= input.MaxRATEDATEFilter)
                        .WhereIf(input.MinCURRATEFilter != null, e => e.CURRATE >= input.MinCURRATEFilter)
                        .WhereIf(input.MaxCURRATEFilter != null, e => e.CURRATE <= input.MaxCURRATEFilter).Where(o => o.TenantId == AbpSession.TenantId);


            var pagedAndFilteredCurrencyRates = filteredCurrencyRates
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var currencyRates = from o in pagedAndFilteredCurrencyRates

                                select new GetCurrencyRateForViewDto()
                                {
                                    CurrencyRate = new CurrencyRateDto
                                    {
                                        // CMPID = o.CMPID,
                                        AUDTDATE = o.AUDTDATE,
                                        AUDTUSER = o.AUDTUSER,
                                        CURNAME = o.CURNAME,
                                        SYMBOL = o.SYMBOL,
                                        RATEDATE = o.RATEDATE,
                                        CURRATE = o.CURRATE,
                                        Id = o.Id
                                    },
                                    //	CompanyProfileCompanyName = s1 == null ? "" : s1.CompanyName.ToString()
                                };

            var totalCount = await filteredCurrencyRates.CountAsync();

            return new PagedResultDto<GetCurrencyRateForViewDto>(
                totalCount,
                await currencyRates.ToListAsync()
            );
        }

        public async Task<GetCurrencyRateForViewDto> GetCurrencyRateForView(string id)
        {
            var currencyRate = await _currencyRateRepository.GetAsync(id);

            var output = new GetCurrencyRateForViewDto { CurrencyRate = ObjectMapper.Map<CurrencyRateDto>(currencyRate) };

            if (output.CurrencyRate.CMPID != null)
            {
                var _lookupCompanyProfile = await _lookup_companyProfileRepository.FirstOrDefaultAsync((string)output.CurrencyRate.CMPID);
                output.CompanyProfileCompanyName = _lookupCompanyProfile.CompanyName.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.SetupForms_CurrencyRates_Edit)]
        public async Task<GetCurrencyRateForEditOutput> GetCurrencyRateForEdit(EntityDto<string> input)
        {
            var currencyRate = await _currencyRateRepository.FirstOrDefaultAsync(input.Id);
            currencyRate.SYMBOL = currencyRate.SYMBOL.Trim();


            var currencyHistory = _currencyHistoryRepository.GetAll().Where(x => x.CurID == currencyRate.Id && x.TenantId == AbpSession.TenantId);

            var output = new GetCurrencyRateForEditOutput
            {
                CurrencyRate = ObjectMapper.Map<CreateOrEditCurrencyRateDto>(currencyRate),
                currencyHistory = ObjectMapper.Map<IEnumerable<CurrencyRateHistoryDto>>(currencyHistory)
            };

            //if (output.CurrencyRate.CMPID != null)
            //      {
            //          var _lookupCompanyProfile = await _lookup_companyProfileRepository.FirstOrDefaultAsync((string)output.CurrencyRate.CMPID);
            //          output.CompanyProfileCompanyName = _lookupCompanyProfile.CompanyName.ToString();
            //      }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCurrencyRateDto input)
        {
            var currencyRate = await _currencyRateRepository.FirstOrDefaultAsync(x => x.Id == input.Id && x.TenantId == AbpSession.TenantId);
            input.SYMBOL = input.SYMBOL.Trim();


            if (currencyRate == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.SetupForms_CurrencyRates_Create)]
        protected virtual async Task Create(CreateOrEditCurrencyRateDto input)
        {
            var currencyRate = ObjectMapper.Map<CurrencyRate>(input);


            if (AbpSession.TenantId != null)
            {
                currencyRate.TenantId = (int)AbpSession.TenantId;
            }
            currencyRate.AUDTUSER = AbpSession.UserId.ToString();
            currencyRate.AUDTDATE = DateTime.Now;

            //if (currencyRate.Id.IsNullOrWhiteSpace())
            //{
            //    currencyRate.Id = Guid.NewGuid().ToString();
            //}

            await _currencyRateRepository.InsertAsync(currencyRate);
        }

        [AbpAuthorize(AppPermissions.SetupForms_CurrencyRates_Edit)]
        protected virtual async Task Update(CreateOrEditCurrencyRateDto input)
        {
            var currencyRate = await _currencyRateRepository.FirstOrDefaultAsync((string)input.Id);
            input.AUDTUSER = AbpSession.UserId.ToString();
            ObjectMapper.Map(input, currencyRate);
        }

        [AbpAuthorize(AppPermissions.SetupForms_CurrencyRates_Delete)]
        public async Task Delete(EntityDto<string> input)
        {
            await _currencyRateRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCurrencyRatesToExcel(GetAllCurrencyRatesForExcelInput input)
        {

            var filteredCurrencyRates = _currencyRateRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CURIDFilter), e => e.Id.ToLower() == input.CURIDFilter.ToLower().Trim())
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CURNAMEFilter), e => e.CURNAME.ToLower() == input.CURNAMEFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SYMBOLFilter), e => e.SYMBOL.ToLower() == input.SYMBOLFilter.ToLower().Trim())
                        .WhereIf(input.MinRATEDATEFilter != null, e => e.RATEDATE >= input.MinRATEDATEFilter)
                        .WhereIf(input.MaxRATEDATEFilter != null, e => e.RATEDATE <= input.MaxRATEDATEFilter)
                        .WhereIf(input.MinCURRATEFilter != null, e => e.CURRATE >= input.MinCURRATEFilter)
                        .WhereIf(input.MaxCURRATEFilter != null, e => e.CURRATE <= input.MaxCURRATEFilter).Where(o => o.TenantId == AbpSession.TenantId);


            var query = (from o in filteredCurrencyRates


                         select new GetCurrencyRateForViewDto()
                         {
                             CurrencyRate = new CurrencyRateDto
                             {
                                 // CMPID = o.CMPID,
                                 AUDTDATE = o.AUDTDATE,
                                 AUDTUSER = o.AUDTUSER,
                                 CURNAME = o.CURNAME,
                                 SYMBOL = o.SYMBOL,
                                 RATEDATE = o.RATEDATE,
                                 CURRATE = o.CURRATE,
                                 Id = o.Id
                             },
                             //CompanyProfileCompanyName = s1 == null ? "" : s1.CompanyName.ToString()
                         });


            var currencyRateListDtos = await query.ToListAsync();

            return _currencyRatesExcelExporter.ExportToFile(currencyRateListDtos);
        }



        [AbpAuthorize(AppPermissions.SetupForms_CurrencyRates)]
        public async Task<PagedResultDto<CurrencyRateCompanyProfileLookupTableDto>> GetAllCompanyProfileForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_companyProfileRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.CompanyName.ToString().Contains(input.Filter)
               ).Where(o => o.TenantId == AbpSession.TenantId);

            var totalCount = await query.CountAsync();

            var companyProfileList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CurrencyRateCompanyProfileLookupTableDto>();
            foreach (var companyProfile in companyProfileList)
            {
                lookupTableDtoList.Add(new CurrencyRateCompanyProfileLookupTableDto
                {
                    Id = companyProfile.Id,
                    DisplayName = companyProfile.CompanyName?.ToString()
                });
            }

            return new PagedResultDto<CurrencyRateCompanyProfileLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}