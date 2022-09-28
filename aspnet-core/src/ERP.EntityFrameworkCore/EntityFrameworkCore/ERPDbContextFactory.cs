using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ERP.Configuration;
using ERP.Web;

namespace ERP.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class ERPDbContextFactory : IDesignTimeDbContextFactory<ERPDbContext>
    {
        public ERPDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ERPDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);

            ERPDbContextConfigurer.Configure(builder, configuration.GetConnectionString(ERPConsts.ConnectionStringName));

            return new ERPDbContext(builder.Options);
        }
    }
}