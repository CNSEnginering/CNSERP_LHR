using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Sales.OERoutes.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Sales.OERoutes
{
    public interface IOERoutesAppService : IApplicationService
    {
        Task<PagedResultDto<GetOERoutesForViewDto>> GetAll(GetAllOERoutesInput input);

        Task<GetOERoutesForViewDto> GetOERoutesForView(int id);

        Task<GetOERoutesForEditOutput> GetOERoutesForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditOERoutesDto input);

        Task Delete(EntityDto input);

    }
}