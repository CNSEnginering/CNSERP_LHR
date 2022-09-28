namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetGLCONFIGForViewDto
    {
		public GLCONFIGDto GLCONFIG { get; set; }

		public string GLBOOKSBookName { get; set;}

		public string ChartofControlAccountName { get; set;}

		public string AccountSubLedgerSubAccName { get; set;}


    }
}