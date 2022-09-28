namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetAccountSubLedgerForViewDto
    {
		public AccountSubLedgerDto AccountSubLedger { get; set; }

		public string ChartofControlAccountName { get; set;}

		public string TaxAuthorityTAXAUTHDESC { get; set;}

        public string ParentAccountName { get; set; }


    }
}