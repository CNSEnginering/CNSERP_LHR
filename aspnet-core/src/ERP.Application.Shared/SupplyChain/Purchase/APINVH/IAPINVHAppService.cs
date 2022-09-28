using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Purchase.APINVH.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Purchase.APINVH
{
    public interface IAPINVHAppService : IApplicationService
    {
        Task<PagedResultDto<GetAPINVHForViewDto>> GetAll(GetAllAPINVHInput input);

        Task<GetAPINVHForViewDto> GetAPINVHForView(int id);

        Task<GetAPINVHForEditOutput> GetAPINVHForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAPINVHDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetAPINVHToExcel(GetAllAPINVHForExcelInput input);

    }
}