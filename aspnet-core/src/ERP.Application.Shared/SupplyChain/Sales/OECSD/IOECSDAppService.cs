using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Sales.OECSD.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Sales.OECSD
{
    public interface IOECSDAppService : IApplicationService
    {
        Task<PagedResultDto<GetOECSDForViewDto>> GetAll(GetAllOECSDInput input);

        Task<GetOECSDForViewDto> GetOECSDForView(int id);

        Task<GetOECSDForEditOutput> GetOECSDForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditOECSDDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetOECSDToExcel(GetAllOECSDForExcelInput input);

    }
}