namespace ERP.MultiTenancy.Payments.PayPal.Dto
{
    public class PayPalConfigurationDto
    {
        public string ClientId { get; set; }

        public string Environment { get; set; }

        public string DemoUsername { get; set; }

        public string DemoPassword { get; set; }
    }
}
