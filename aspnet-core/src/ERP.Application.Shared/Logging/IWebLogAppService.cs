using Abp.Application.Services;
using ERP.Dto;
using ERP.Logging.Dto;

namespace ERP.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
