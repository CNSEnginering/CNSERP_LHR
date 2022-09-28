using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Sales.SaleQutation.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Sales.SaleQutation
{
    public interface IOEQDAppService : IApplicationService
    {
        Task<PagedResultDto<GetOEQDForViewDto>> GetAll(GetAllOEQDInput input);

        Task<GetOEQDForViewDto> GetOEQDForView(int id);

        Task<GetOEQDForEditOutput> GetOEQDForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditOEQDDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetOEQDToExcel(GetAllOEQDForExcelInput input);

    }
}