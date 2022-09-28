using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Sales.SaleQutation.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Sales.SaleQutation
{
    public interface IOEQHAppService : IApplicationService
    {
        Task<PagedResultDto<GetOEQHForViewDto>> GetAll(GetAllOEQHInput input);

        Task<GetOEQHForViewDto> GetOEQHForView(int id);

        Task<GetOEQHForEditOutput> GetOEQHForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditOEQHDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetOEQHToExcel(GetAllOEQHForExcelInput input);

    }
}