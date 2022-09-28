using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.Manufacturing.SetupForms
{
    public interface IMFWCTOLAppService : IApplicationService
    {
        Task<PagedResultDto<GetMFWCTOLForViewDto>> GetAll(GetAllMFWCTOLInput input);

        Task<GetMFWCTOLForViewDto> GetMFWCTOLForView(int id);

        Task<GetMFWCTOLForEditOutput> GetMFWCTOLForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditMFWCTOLDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetMFWCTOLToExcel(GetAllMFWCTOLForExcelInput input);

    }
}