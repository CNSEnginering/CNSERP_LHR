using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Dto;
using ERP.SupplyChain.Inventory.IC_Item.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.SupplyChain.Inventory.IC_Item
{
    public interface IICItemAppService : IApplicationService
    {
        Task<PagedResultDto<GetIcItemForViewDto>> GetAll(GetAllIcItemInput input);

        Task<GetIcItemForViewDto> GetIcItemForView(int id);

        Task<GetIcItemForEditOutput> GetIcItemForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditIcItemDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetIcItemToExcel(GetAllIcItemForExcelInput input);


        string GetName(string Id);
    }
}
