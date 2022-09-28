using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Authorization.Permissions.Dto;

namespace ERP.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
