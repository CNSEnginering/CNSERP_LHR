using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms
{
    public interface ICurrencyRatesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCurrencyRateForViewDto>> GetAll(GetAllCurrencyRatesInput input);

        Task<GetCurrencyRateForViewDto> GetCurrencyRateForView(string id);

		Task<GetCurrencyRateForEditOutput> GetCurrencyRateForEdit(EntityDto<string> input);

		Task CreateOrEdit(CreateOrEditCurrencyRateDto input);

		Task Delete(EntityDto<string> input);

		Task<FileDto> GetCurrencyRatesToExcel(GetAllCurrencyRatesForExcelInput input);

		
		Task<PagedResultDto<CurrencyRateCompanyProfileLookupTableDto>> GetAllCompanyProfileForLookupTable(GetAllForLookupTableInput input);
		
    }
}