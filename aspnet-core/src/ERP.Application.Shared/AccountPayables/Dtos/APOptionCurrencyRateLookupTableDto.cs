using Abp.Application.Services.Dto;

namespace ERP.AccountPayables.Dtos
{
    public class APOptionCurrencyRateLookupTableDto
    {
		public string Id { get; set; }

		public string DisplayName { get; set; }

        public double CurrRate { get; set; }

        public string Symbol { get; set; }
    }
}