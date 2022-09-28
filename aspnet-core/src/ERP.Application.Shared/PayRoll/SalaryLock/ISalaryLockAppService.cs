using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.SalaryLock.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.SalaryLock
{
    public interface ISalaryLockAppService : IApplicationService
    {
        Task<PagedResultDto<GetSalaryLockForViewDto>> GetAll(GetAllSalaryLockInput input);

        Task<GetSalaryLockForViewDto> GetSalaryLockForView(int id);

        Task<GetSalaryLockForEditOutput> GetSalaryLockForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditSalaryLockDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetSalaryLockToExcel(GetAllSalaryLockForExcelInput input);

    }
}