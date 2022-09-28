using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Sales.OECSH.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Sales.OECSH
{
    public interface IOECSHAppService : IApplicationService
    {
        Task<PagedResultDto<GetOECSHForViewDto>> GetAll(GetAllOECSHInput input);

        Task<GetOECSHForViewDto> GetOECSHForView(int id);

        Task<GetOECSHForEditOutput> GetOECSHForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditOECSHDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetOECSHToExcel(GetAllOECSHForExcelInput input);

    }
}