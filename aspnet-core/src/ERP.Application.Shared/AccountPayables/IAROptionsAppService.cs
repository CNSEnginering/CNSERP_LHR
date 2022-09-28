using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.AccountPayables.Dtos;
using ERP.Dto;

namespace ERP.AccountPayables
{
    public interface IAROptionsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAROptionForViewDto>> GetAll(GetAllAROptionsInput input);

        Task<GetAROptionForViewDto> GetAROptionForView(int id);

		Task<GetAROptionForEditOutput> GetAROptionForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditAROptionDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetAROptionsToExcel(GetAllAROptionsForExcelInput input);

		
		Task<PagedResultDto<AROptionBankLookupTableDto>> GetAllBankForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<AROptionCurrencyRateLookupTableDto>> GetAllCurrencyRateForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<AROptionChartofControlLookupTableDto>> GetAllChartofControlForLookupTable(GetAllForLookupTableInput input);
		
    }
}