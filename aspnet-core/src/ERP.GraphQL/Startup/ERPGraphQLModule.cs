using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace ERP.Startup
{
    [DependsOn(typeof(ERPCoreModule))]
    public class ERPGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ERPGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}