using Abp.Domain.Services;

namespace ERP
{
    public abstract class ERPDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected ERPDomainServiceBase()
        {
            LocalizationSourceName = ERPConsts.LocalizationSourceName;
        }
    }
}
