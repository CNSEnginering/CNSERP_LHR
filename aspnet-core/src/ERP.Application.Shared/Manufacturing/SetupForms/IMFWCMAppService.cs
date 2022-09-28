using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.Manufacturing.SetupForms
{
    public interface IMFWCMAppService : IApplicationService
    {
        Task<PagedResultDto<GetMFWCMForViewDto>> GetAll(GetAllMFWCMInput input);

        Task<GetMFWCMForEditOutput> GetMFWCMForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditMFWCMDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetMFWCMToExcel(GetAllMFWCMForExcelInput input);

    }
}