using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Sales.Invoices.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Sales.Invoices
{
    public interface IOEINVKNOCKDAppService : IApplicationService
    {
        Task<PagedResultDto<GetOEINVKNOCKDForViewDto>> GetAll(GetAllOEINVKNOCKDInput input);

        Task<GetOEINVKNOCKDForViewDto> GetOEINVKNOCKDForView(int id);

        Task<GetOEINVKNOCKDForEditOutput> GetOEINVKNOCKDForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditOEINVKNOCKDDto input);

        Task Delete(EntityDto input);

    }
}