using System;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using ERP.Authorization;
using ERP.Editions;
using ERP.MultiTenancy.Payments.Stripe;
using ERP.MultiTenancy.Payments.Stripe.Dto;

namespace ERP.MultiTenancy.Payments
{
    public class StripePaymentAppService : ERPAppServiceBase, IStripePaymentAppService
    {
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly StripeGatewayManager _stripeGatewayManager;
        private readonly StripePaymentGatewayConfiguration _strieStripePaymentGatewayConfiguration;
        private readonly IRepository<SubscribableEdition> _editionRepository;
        private readonly TenantManager _tenantManager;

        public StripePaymentAppService(
            StripeGatewayManager stripeGatewayManager,
            StripePaymentGatewayConfiguration strieStripePaymentGatewayConfiguration,
            IRepository<SubscribableEdition> editionRepository,
            TenantManager tenantManager,
            ISubscriptionPaymentRepository subscriptionPaymentRepository)
        {
            _stripeGatewayManager = stripeGatewayManager;
            _strieStripePaymentGatewayConfiguration = strieStripePaymentGatewayConfiguration;
            _editionRepository = editionRepository;
            _tenantManager = tenantManager;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
        }

        public async Task ConfirmPayment(StripeConfirmPaymentInput input)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(input.PaymentId);
            if (payment.Status != SubscriptionPaymentStatus.NotPaid)
            {
                throw new ApplicationException($"Invalid payment status {payment.Status}, cannot create a charge on stripe !");
            }

            var result = await _stripeGatewayManager.CreateCharge(input.StripeToken, payment.Amount, payment.Description);

            payment.Gateway = SubscriptionPaymentGatewayType.Stripe;
            payment.ExternalPaymentId = result.Id;
            payment.SetAsPaid();
        }

        public async Task CreateSubscription(StripeCreateSubscriptionInput input)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(input.PaymentId);
            var edition = await _editionRepository.GetAsync(payment.EditionId);

            var product = await _stripeGatewayManager.GetOrCreateProductAsync(StripeGatewayManager.ProductName);
            
            var planId = _stripeGatewayManager.GetPlanId(edition.Name, payment.GetPaymentPeriodType());
            var planInterval = _stripeGatewayManager.GetPlanInterval(payment.PaymentPeriodType);

            var plan = await _stripeGatewayManager.GetOrCreatePlanAsync(planId, payment.Amount, planInterval, product.Id);

            Tenant tenant;

            using (CurrentUnitOfWork.SetTenantId(null))
            {
                tenant = await _tenantManager.GetByIdAsync(payment.TenantId);
            }

            var adminUser = await UserManager.GetAdminAsync();
            if (adminUser == null)
            {
                throw new ApplicationException("There is no admin user for current Tenant");
            }

            var customer = await _stripeGatewayManager.CreateCustomerAsync(tenant.TenancyName, adminUser.EmailAddress, input.StripeToken);

            var subscriptionResult = await _stripeGatewayManager.CreateSubscription(customer.Id, plan.Id);

            payment.Gateway = SubscriptionPaymentGatewayType.Stripe;
            payment.ExternalPaymentId = subscriptionResult.Id;
            payment.SetAsPaid();
        }

        public async Task UpdateSubscription(StripeUpdateSubscriptionInput input)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(input.PaymentId);
            var edition = await _editionRepository.GetAsync(payment.EditionId);

            var lastPayment = await _subscriptionPaymentRepository.GetLastCompletedPaymentOrDefaultAsync(
                tenantId: AbpSession.GetTenantId(),
                SubscriptionPaymentGatewayType.Stripe,
                isRecurring: true);

            if (lastPayment == null)
            {
                throw new ApplicationException("You don't have a valid subscription !");
            }

            var newPlanId = _stripeGatewayManager.GetPlanId(
                edition.Name,
                lastPayment.GetPaymentPeriodType()
            );

            await _stripeGatewayManager.UpdateSubscription(
                lastPayment.ExternalPaymentId,
                newPlanId,
                edition.GetPaymentAmount(payment.GetPaymentPeriodType()),
                _stripeGatewayManager.GetPlanInterval(payment.GetPaymentPeriodType())
            );
        }

        public StripeConfigurationDto GetConfiguration()
        {
            return new StripeConfigurationDto
            {
                PublishableKey = _strieStripePaymentGatewayConfiguration.PublishableKey
            };
        }
    }
}