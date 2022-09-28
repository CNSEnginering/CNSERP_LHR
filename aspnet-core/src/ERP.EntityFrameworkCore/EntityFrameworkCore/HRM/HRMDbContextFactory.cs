using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ERP.Configuration;
using ERP.Web;

namespace ERP.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class HRMDbContextFactory : IDesignTimeDbContextFactory<HRMDbContext>
    {
        public HRMDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<HRMDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);

            HRMDbContextConfigurer.Configure(builder, configuration.GetConnectionString(ERPConsts.ConnectionStringName));

            return new HRMDbContext(builder.Options);
        }
    }
}