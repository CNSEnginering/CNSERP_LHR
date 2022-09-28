using Abp.AspNetCore.Mvc.Views;

namespace ERP.Web.Views
{
    public abstract class ERPRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected ERPRazorPage()
        {
            LocalizationSourceName = ERPConsts.LocalizationSourceName;
        }
    }
}
