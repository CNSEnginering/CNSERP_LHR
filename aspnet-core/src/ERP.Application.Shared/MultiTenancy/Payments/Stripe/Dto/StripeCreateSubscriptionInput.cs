namespace ERP.MultiTenancy.Payments.Stripe.Dto
{
    public class StripeCreateSubscriptionInput
    {
        public long PaymentId { get; set; }

        public string StripeToken { get; set; }
    }
}