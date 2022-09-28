using Abp.AspNetCore.Mvc.ViewComponents;

namespace ERP.Web.Public.Views
{
    public abstract class ERPViewComponent : AbpViewComponent
    {
        protected ERPViewComponent()
        {
            LocalizationSourceName = ERPConsts.LocalizationSourceName;
        }
    }
}