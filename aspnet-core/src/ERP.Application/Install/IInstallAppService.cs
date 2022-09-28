using System.Threading.Tasks;
using Abp.Application.Services;
using ERP.Install.Dto;

namespace ERP.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}