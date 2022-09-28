using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.CommonServices.Dtos;
using ERP.Dto;

namespace ERP.CommonServices
{
    public interface IBkTransfersAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBkTransferForViewDto>> GetAll(GetAllBkTransfersInput input);

        Task<GetBkTransferForViewDto> GetBkTransferForView(int id);

		Task<GetBkTransferForEditOutput> GetBkTransferForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditBkTransferDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetBkTransfersToExcel(GetAllBkTransfersForExcelInput input);

		
		Task<PagedResultDto<BkTransferBankLookupTableDto>> GetAllBankForLookupTable(GetAllForLookupTableInput input);
		
    }
}