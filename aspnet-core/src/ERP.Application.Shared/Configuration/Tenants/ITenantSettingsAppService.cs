using System.Threading.Tasks;
using Abp.Application.Services;
using ERP.Configuration.Tenants.Dto;

namespace ERP.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
