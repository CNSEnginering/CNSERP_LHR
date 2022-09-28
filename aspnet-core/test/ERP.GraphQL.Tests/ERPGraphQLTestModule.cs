using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ERP.Configure;
using ERP.Startup;
using ERP.Test.Base;

namespace ERP.GraphQL.Tests
{
    [DependsOn(
        typeof(ERPGraphQLModule),
        typeof(ERPTestBaseModule))]
    public class ERPGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ERPGraphQLTestModule).GetAssembly());
        }
    }
}