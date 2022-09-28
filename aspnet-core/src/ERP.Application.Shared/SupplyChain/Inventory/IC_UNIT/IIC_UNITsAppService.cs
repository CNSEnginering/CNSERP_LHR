using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.IC_UNIT.Dto;
using ERP.Dto;
using System.Collections.Generic;

namespace ERP.SupplyChain.Inventory.IC_UNIT
{
    public interface IIC_UNITsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetIC_UNITForViewDto>> GetAll(GetAllIC_UNITsInput input);

		Task<GetIC_UNITForEditOutput> GetIC_UNITForEdit(string ItemID);

		Task CreateOrEdit(ICollection<CreateOrEditIC_UNITDto> input);

		Task Delete(EntityDto input);

        Task<ListResultDto<IC_UNITDto>> GetUnitList();


    }
}