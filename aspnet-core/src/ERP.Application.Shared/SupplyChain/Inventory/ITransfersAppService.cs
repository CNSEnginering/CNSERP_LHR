using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory
{
    public interface ITransfersAppService : IApplicationService 
    {
         Task<PagedResultDto<GetTransfersForViewDto>> GetAll(GetAllTransfersInput input);

        Task<GetTransfersForViewDto> GetTransferForView(int id);

		Task<GetTransferForEditOutput> GetTransferForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditTransferDto input);

		Task Delete(EntityDto input);

		
    }
}