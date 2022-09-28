using System.Threading.Tasks;
using ERP.MultiTenancy.Payments.Dto;
using ERP.MultiTenancy.Payments.Paypal;
using ERP.MultiTenancy.Payments.PayPal;
using ERP.MultiTenancy.Payments.PayPal.Dto;

namespace ERP.MultiTenancy.Payments
{
    public class PayPalPaymentAppService : ERPAppServiceBase, IPayPalPaymentAppService
    {
        private readonly PayPalGatewayManager _payPalGatewayManager;
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly PayPalPaymentGatewayConfiguration _payPalPaymentGatewayConfiguration;

        public PayPalPaymentAppService(
            PayPalGatewayManager payPalGatewayManager,
            ISubscriptionPaymentRepository subscriptionPaymentRepository, 
            PayPalPaymentGatewayConfiguration payPalPaymentGatewayConfiguration)
        {
            _payPalGatewayManager = payPalGatewayManager;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _payPalPaymentGatewayConfiguration = payPalPaymentGatewayConfiguration;
        }

        public async Task ConfirmPayment(long paymentId, string paypalPaymentId, string paypalPayerId)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(paymentId);

            await _payPalGatewayManager.ExecutePaymentAsync(
                new PayPalExecutePaymentRequestInput(paypalPaymentId, paypalPayerId)
            );

            payment.Gateway = SubscriptionPaymentGatewayType.Paypal;
            payment.ExternalPaymentId = paypalPaymentId;
            payment.SetAsPaid();
        }

        public PayPalConfigurationDto GetConfiguration()
        {
            return new PayPalConfigurationDto
            {
                Environment = _payPalPaymentGatewayConfiguration.Environment,
                ClientId = _payPalPaymentGatewayConfiguration.ClientId,
                DemoUsername = _payPalPaymentGatewayConfiguration.DemoUsername,
                DemoPassword = _payPalPaymentGatewayConfiguration.DemoPassword
            };
        }

        public async Task CancelPayment(CancelPaymentDto input)
        {
            var payment = await _subscriptionPaymentRepository.GetByGatewayAndPaymentIdAsync(
                input.Gateway,
                input.PaymentId
            );

            payment.SetAsCancelled();
        }
    }
}