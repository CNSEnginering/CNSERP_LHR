using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Sales.Invoices.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Sales.Invoices
{
    public interface IOEINVKNOCKHAppService : IApplicationService
    {
        Task<PagedResultDto<GetOEINVKNOCKHForViewDto>> GetAll(GetAllOEINVKNOCKHInput input);

        Task<GetOEINVKNOCKHForViewDto> GetOEINVKNOCKHForView(int id);

        Task<CreateOrEditOEINVKNOCKHDto> GetOEINVKNOCKHForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditOEINVKNOCKHDto input);

        Task Delete(EntityDto input);

    }
}