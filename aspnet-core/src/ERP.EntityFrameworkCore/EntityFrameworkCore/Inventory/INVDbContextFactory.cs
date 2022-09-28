using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ERP.Configuration;
using ERP.Web;

namespace ERP.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class INVDbContextFactory : IDesignTimeDbContextFactory<INVDbContext>
    {
        public INVDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<INVDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);

            INVDbContextConfigurer.Configure(builder, configuration.GetConnectionString(ERPConsts.ConnectionStringName));

            return new INVDbContext(builder.Options);
        }
    }
}