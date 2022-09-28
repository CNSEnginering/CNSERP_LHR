namespace ERP.AccountPayables.Dtos
{
    public class GetAPOptionForViewDto
    {
		public APOptionDto APOption { get; set; }

		public string CurrencyRateId { get; set;}

		public string BankBANKID { get; set;}

		public string ChartofControlId { get; set;}


    }
}