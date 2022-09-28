using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.Manufacturing.SetupForms
{
    public interface IMFTOOLTYAppService : IApplicationService
    {
        Task<PagedResultDto<GetMFTOOLTYForViewDto>> GetAll(GetAllMFTOOLTYInput input);

        Task<GetMFTOOLTYForViewDto> GetMFTOOLTYForView(int id);

        Task<GetMFTOOLTYForEditOutput> GetMFTOOLTYForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditMFTOOLTYDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetMFTOOLTYToExcel(GetAllMFTOOLTYForExcelInput input);

    }
}