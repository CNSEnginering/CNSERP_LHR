using System;
using System.Threading.Tasks;
using Abp.Events.Bus;
using Abp.Runtime.Session;
using ERP.Editions;
using ERP.MultiTenancy.Payments;

namespace ERP.MultiTenancy
{
    public class SubscriptionAppService : ERPAppServiceBase, ISubscriptionAppService
    {
        private readonly TenantManager _tenantManager;
        private readonly EditionManager _editionManager;
        public IEventBus EventBus { get; set; }

        public SubscriptionAppService(
            TenantManager tenantManager,
            EditionManager editionManager)
        {
            _tenantManager = tenantManager;
            _editionManager = editionManager;
            EventBus = NullEventBus.Instance;
        }

        public async Task UpgradeTenantToEquivalentEdition(int upgradeEditionId)
        {
            if (await UpgradeIsFree(upgradeEditionId))
            {
                await _tenantManager.UpdateTenantAsync(
                    AbpSession.GetTenantId(), true, false, null,
                    upgradeEditionId,
                    EditionPaymentType.Upgrade
                );
            }
        }

        public async Task DisableRecurringPayments()
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var tenant = await TenantManager.GetByIdAsync(AbpSession.GetTenantId());
                if (tenant.SubscriptionPaymentType == SubscriptionPaymentType.RecurringAutomatic)
                {
                    tenant.SubscriptionPaymentType = SubscriptionPaymentType.RecurringManual;
                    EventBus.Trigger(new RecurringPaymentsDisabledEventData
                    {
                        TenantId = AbpSession.GetTenantId(),
                        EditionId = tenant.EditionId.Value
                    });
                }
            }
        }

        public async Task EnableRecurringPayments()
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var tenant = await TenantManager.GetByIdAsync(AbpSession.GetTenantId());
                if (tenant.SubscriptionPaymentType == SubscriptionPaymentType.RecurringManual)
                {
                    tenant.SubscriptionPaymentType = SubscriptionPaymentType.RecurringAutomatic;
                    tenant.SubscriptionEndDateUtc = null;

                    EventBus.Trigger(new RecurringPaymentsEnabledEventData
                    {
                        TenantId = AbpSession.GetTenantId()
                    });
                }
            }
        }

        private async Task<bool> UpgradeIsFree(int upgradeEditionId)
        {
            var tenant = await _tenantManager.GetByIdAsync(AbpSession.GetTenantId());

            if (!tenant.EditionId.HasValue)
            {
                throw new Exception("Tenant must be assigned to an Edition in order to upgrade !");
            }

            var currentEdition = (SubscribableEdition)await _editionManager.GetByIdAsync(tenant.EditionId.Value);
            var targetEdition = (SubscribableEdition)await _editionManager.GetByIdAsync(upgradeEditionId);
            var bothEditionsAreFree = targetEdition.IsFree && currentEdition.IsFree;
            var bothEditionsHasSamePrice = currentEdition.HasSamePrice(targetEdition);
            return bothEditionsAreFree || bothEditionsHasSamePrice;
        }
    }
}