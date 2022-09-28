using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Sales.OEDrivers.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Sales.OEDrivers
{
    public interface IOEDriversAppService : IApplicationService
    {
        Task<PagedResultDto<GetOEDriversForViewDto>> GetAll(GetAllOEDriversInput input);

        Task<GetOEDriversForViewDto> GetOEDriversForView(int id);

        Task<GetOEDriversForEditOutput> GetOEDriversForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditOEDriversDto input);

        Task Delete(EntityDto input);

    }
}