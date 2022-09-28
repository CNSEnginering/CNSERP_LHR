using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.ICLOT.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory.ICLOT
{
    public interface IICLOTAppService : IApplicationService
    {
        Task<PagedResultDto<GetICLOTForViewDto>> GetAll(GetAllICLOTInput input);

        Task<GetICLOTForViewDto> GetICLOTForView(int id);

        Task<GetICLOTForEditOutput> GetICLOTForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditICLOTDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetICLOTToExcel(GetAllICLOTForExcelInput input);

    }
}