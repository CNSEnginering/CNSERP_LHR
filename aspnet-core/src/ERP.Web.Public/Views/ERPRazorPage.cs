using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace ERP.Web.Public.Views
{
    public abstract class ERPRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected ERPRazorPage()
        {
            LocalizationSourceName = ERPConsts.LocalizationSourceName;
        }
    }
}
