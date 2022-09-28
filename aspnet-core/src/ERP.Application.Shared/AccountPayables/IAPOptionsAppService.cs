using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.AccountPayables.Dtos;
using ERP.Dto;

namespace ERP.AccountPayables
{
    public interface IAPOptionsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAPOptionForViewDto>> GetAll(GetAllAPOptionsInput input);

        Task<GetAPOptionForViewDto> GetAPOptionForView(int id);


        Task<GetAPOptionForEditOutput> GetAPOptionForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditAPOptionDto input);
        CompanyProfileViewDto GetCompanyProfileData();
		Task Delete(EntityDto input);

		Task<FileDto> GetAPOptionsToExcel(GetAllAPOptionsForExcelInput input);

		
		Task<PagedResultDto<APOptionCurrencyRateLookupTableDto>> GetAllCurrencyRateForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<APOptionBankLookupTableDto>> GetAllBankForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<APOptionChartofControlLookupTableDto>> GetAllChartofControlForLookupTable(GetAllForLookupTableInput input);
		
    }
}