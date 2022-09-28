using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.CommonServices.Dtos;
using ERP.Dto;

namespace ERP.CommonServices
{
    public interface IBanksAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBankForViewDto>> GetAll(GetAllBanksInput input);

        Task<GetBankForViewDto> GetBankForView(int id);

		Task<GetBankForEditOutput> GetBankForEdit(EntityDto input);

        Task<string> CreateOrEdit(CreateOrEditBankDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetBanksToExcel(GetAllBanksForExcelInput input);

		
		Task<PagedResultDto<BankChartofControlLookupTableDto>> GetAllChartofControlForLookupTable(GetAllForLookupTableInput input);
		
    }
}