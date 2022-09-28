using Abp.Modules;
using Abp.Reflection.Extensions;

namespace ERP
{
    public class ERPClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ERPClientModule).GetAssembly());
        }
    }
}
