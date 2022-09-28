using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.Manufacturing.SetupForms
{
    public interface IMFTOOLAppService : IApplicationService
    {
        Task<PagedResultDto<GetMFTOOLForViewDto>> GetAll(GetAllMFTOOLInput input);

        Task<GetMFTOOLForViewDto> GetMFTOOLForView(int id);

        Task<GetMFTOOLForEditOutput> GetMFTOOLForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditMFTOOLDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetMFTOOLToExcel(GetAllMFTOOLForExcelInput input);

    }
}