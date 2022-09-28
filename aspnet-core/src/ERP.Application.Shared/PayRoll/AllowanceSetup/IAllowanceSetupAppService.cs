using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.AllowanceSetup.Dtos;
using ERP.Dto;


namespace ERP.PayRoll.AllowanceSetup
{
    public interface IAllowanceSetupAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAllowanceSetupForViewDto>> GetAll(GetAllAllowanceSetupInput input);

        Task<GetAllowanceSetupForViewDto> GetAllowanceSetupForView(int id);

		Task<GetAllowanceSetupForEditOutput> GetAllowanceSetupForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditAllowanceSetupDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetAllowanceSetupToExcel(GetAllAllowanceSetupForExcelInput input);

		
    }
}