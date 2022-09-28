using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Payroll.SlabSetup.Dtos;
using ERP.Dto;

namespace ERP.Payroll.SlabSetup
{
    public interface ISlabSetupAppService : IApplicationService
    {
        Task<PagedResultDto<GetSlabSetupForViewDto>> GetAll(GetAllSlabSetupInput input);

        Task<GetSlabSetupForViewDto> GetSlabSetupForView(int id);

        Task<GetSlabSetupForEditOutput> GetSlabSetupForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditSlabSetupDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetSlabSetupToExcel(GetAllSlabSetupForExcelInput input);

    }
}