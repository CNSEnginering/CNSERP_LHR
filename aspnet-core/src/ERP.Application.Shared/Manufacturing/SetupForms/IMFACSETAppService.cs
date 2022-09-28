using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.Manufacturing.SetupForms
{
    public interface IMFACSETAppService : IApplicationService
    {
        Task<PagedResultDto<GetMFACSETForViewDto>> GetAll(GetAllMFACSETInput input);

        Task<GetMFACSETForViewDto> GetMFACSETForView(int id);

        Task<GetMFACSETForEditOutput> GetMFACSETForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditMFACSETDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetMFACSETToExcel(GetAllMFACSETForExcelInput input);

    }
}