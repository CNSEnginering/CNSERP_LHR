using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.WorkOrder.Dtos;
using ERP.Dto;


namespace ERP.SupplyChain.Inventory.WorkOrder
{
    public interface IICWODetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<ICWODetailDto>> GetICWODData(int detId, string ccId);

		Task<ICWODetailDto> GetICWODetailForEdit(EntityDto input);

    }
}