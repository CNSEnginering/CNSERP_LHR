using Abp;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Configuration;
using Abp.IdentityServer4;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using ERP.Configuration;
using ERP.EntityHistory;
using ERP.Migrations.Seed;
using Abp.Configuration.Startup;

namespace ERP.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpZeroCoreEntityFrameworkCoreModule),
        typeof(ERPCoreModule),
        typeof(AbpZeroCoreIdentityServerEntityFrameworkCoreModule)
        )]
    public class ERPEntityFrameworkCoreModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            Configuration.ReplaceService<IConnectionStringResolver, MultiDbConnectionStringResolver>();
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<ERPDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        ERPDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        ERPDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });

                // Configure INV DbContext
                Configuration.Modules.AbpEfCore().AddDbContext<INVDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        INVDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        INVDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });

                // Configure Payroll DbContext
                Configuration.Modules.AbpEfCore().AddDbContext<HRMDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        HRMDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        HRMDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        

            // Uncomment below line to write change logs for the entities below:
            //Configuration.EntityHistory.Selectors.Add("ERPEntities", EntityHistoryHelper.TrackedTypes);
            //Configuration.CustomConfigProviders.Add(new EntityHistoryConfigProvider(Configuration));
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ERPEntityFrameworkCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            var configurationAccessor = IocManager.Resolve<IAppConfigurationAccessor>();

            using (var scope = IocManager.CreateScope())
            {
                if (!SkipDbSeed && scope.Resolve<DatabaseCheckHelper>().Exist(configurationAccessor.Configuration["ConnectionStrings:Default"]))
                {
                    SeedHelper.SeedHostDb(IocManager);
                }
            }
        }
    }
}
