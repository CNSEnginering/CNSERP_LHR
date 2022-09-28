using System.Threading.Tasks;
using Abp.Application.Services;
using ERP.MultiTenancy.Payments.Dto;
using ERP.MultiTenancy.Payments.PayPal.Dto;

namespace ERP.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalPaymentId, string paypalPayerId);

        PayPalConfigurationDto GetConfiguration();

        Task CancelPayment(CancelPaymentDto input);
    }
}
