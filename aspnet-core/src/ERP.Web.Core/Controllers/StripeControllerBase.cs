using System;
using System.IO;
using System.Threading.Tasks;
using Abp;
using Microsoft.AspNetCore.Mvc;
using ERP.MultiTenancy.Payments.Stripe;
using Stripe;

namespace ERP.Web.Controllers
{
    public class StripeControllerBase : ERPControllerBase
    {
        private readonly StripeGatewayManager _stripeGatewayManager;
        private readonly StripePaymentGatewayConfiguration _stripeConfiguration;

        public StripeControllerBase(
            StripeGatewayManager stripeGatewayManager,
            StripePaymentGatewayConfiguration stripeConfiguration)
        {
            _stripeGatewayManager = stripeGatewayManager;
            _stripeConfiguration = stripeConfiguration;
        }

        [HttpPost]
        public async Task<IActionResult> WebHooks()
        {
            var json = new StreamReader(HttpContext.Request.Body).ReadToEnd();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _stripeConfiguration.WebhookSecret);

                if (stripeEvent.Type == Events.InvoicePaymentSucceeded)
                {
                    await HandleSubscriptionCyclePaymentAsync(stripeEvent);
                }

                // Other WebHook events can be handled here.

                return Ok();
            }
            catch (ApplicationException exception)
            {
                Logger.Error(exception.Message, exception);
                return BadRequest();
            }
            catch (StripeException exception)
            {
                Logger.Error(exception.Message, exception);
                return BadRequest();
            }
        }

        private async Task HandleSubscriptionCyclePaymentAsync(Event stripeEvent)
        {
            var invoice = stripeEvent.Data.Object as Invoice;
            if (invoice == null)
            {
                throw new ApplicationException("Unable to get Invoice information from stripeEvent.Data");
            }

            // see https://stripe.com/docs/api/invoices/object#invoice_object-billing_reason
            // only handle for subscription_cycle payments
            if (invoice.BillingReason == "subscription_cycle")
            {
                await _stripeGatewayManager.HandleInvoicePaymentSucceededAsync(invoice);
            }
        }
    }
}
