using Microsoft.Extensions.Configuration;

namespace ERP.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
