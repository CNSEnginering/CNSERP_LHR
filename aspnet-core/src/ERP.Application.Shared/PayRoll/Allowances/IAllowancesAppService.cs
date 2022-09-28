using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.Allowances.Dtos;
using ERP.Dto;


namespace ERP.PayRoll.Allowances
{
    public interface IAllowancesAppService : IApplicationService
    {
        Task<PagedResultDto<GetAllowancesForViewDto>> GetAll(GetAllAllowancesInput input);

        Task<GetAllowancesForViewDto> GetAllowancesForView(int id);

        Task<GetAllowancesForEditOutput> GetAllowancesForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAllowancesDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetAllowancesToExcel(GetAllAllowancesForExcelInput input);

    }
}