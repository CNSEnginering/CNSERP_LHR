using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.Manufacturing.SetupForms
{
    public interface IMFWCRESAppService : IApplicationService
    {
        Task<PagedResultDto<GetMFWCRESForViewDto>> GetAll(GetAllMFWCRESInput input);

        Task<GetMFWCRESForViewDto> GetMFWCRESForView(int id);

        Task<GetMFWCRESForEditOutput> GetMFWCRESForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditMFWCRESDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetMFWCRESToExcel(GetAllMFWCRESForExcelInput input);

    }
}