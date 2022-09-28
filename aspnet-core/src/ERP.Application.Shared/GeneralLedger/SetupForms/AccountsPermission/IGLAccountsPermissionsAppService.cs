using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.AccountsPermission.Dtos;
using ERP.Dto;


namespace ERP.GeneralLedger.SetupForms.AccountsPermission
{
    public interface IGLAccountsPermissionsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetGLAccountsPermissionForViewDto>> GetAll(GetAllGLAccountsPermissionsInput input);

        Task<GetGLAccountsPermissionForViewDto> GetGLAccountsPermissionForView(int id);

		Task<GetGLAccountsPermissionForEditOutput> GetGLAccountsPermissionForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditGLAccountsPermissionDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetGLAccountsPermissionsToExcel(GetAllGLAccountsPermissionsForExcelInput input);

		
    }
}