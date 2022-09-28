using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.Manufacturing.SetupForms
{
    public interface IMFAREAAppService : IApplicationService
    {
        Task<PagedResultDto<GetMFAREAForViewDto>> GetAll(GetAllMFAREAInput input);

        Task<GetMFAREAForViewDto> GetMFAREAForView(int id);

        Task<GetMFAREAForEditOutput> GetMFAREAForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditMFAREADto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetMFAREAToExcel(GetAllMFAREAForExcelInput input);

    }
}