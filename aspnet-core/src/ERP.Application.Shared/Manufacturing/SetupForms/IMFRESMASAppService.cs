using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.Manufacturing.SetupForms
{
    public interface IMFRESMASAppService : IApplicationService
    {
        Task<PagedResultDto<GetMFRESMASForViewDto>> GetAll(GetAllMFRESMASInput input);

        Task<GetMFRESMASForViewDto> GetMFRESMASForView(int id);

        Task<GetMFRESMASForEditOutput> GetMFRESMASForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditMFRESMASDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetMFRESMASToExcel(GetAllMFRESMASForExcelInput input);

    }
}