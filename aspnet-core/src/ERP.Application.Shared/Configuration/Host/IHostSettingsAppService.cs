using System.Threading.Tasks;
using Abp.Application.Services;
using ERP.Configuration.Host.Dto;

namespace ERP.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
