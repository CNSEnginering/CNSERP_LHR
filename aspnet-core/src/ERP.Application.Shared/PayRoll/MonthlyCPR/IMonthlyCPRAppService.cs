using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.MonthlyCPR.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.MonthlyCPR
{
    public interface IMonthlyCPRAppService : IApplicationService
    {
        Task<PagedResultDto<GetMonthlyCPRForViewDto>> GetAll(GetAllMonthlyCPRInput input);

        Task<GetMonthlyCPRForViewDto> GetMonthlyCPRForView(int id);

        Task<GetMonthlyCPRForEditOutput> GetMonthlyCPRForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditMonthlyCPRDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetMonthlyCPRToExcel(GetAllMonthlyCPRForExcelInput input);

    }
}