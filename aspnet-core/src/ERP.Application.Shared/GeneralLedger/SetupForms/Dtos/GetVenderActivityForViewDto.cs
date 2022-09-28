namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetVenderActivityForViewDto
    {
        public VenderActivityDto VenderActivity { get; set; }

        public decimal OpeningBalance { get; set; }
        public decimal OutstandingBalance { get; set; }
        public double? LastPayment { get; set; }
    }
}