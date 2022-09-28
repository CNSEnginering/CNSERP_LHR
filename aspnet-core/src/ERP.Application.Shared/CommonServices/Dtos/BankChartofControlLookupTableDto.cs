using Abp.Application.Services.Dto;

namespace ERP.CommonServices.Dtos
{
    public class BankChartofControlLookupTableDto
    {
		public string Id { get; set; }

		public string DisplayName { get; set; }

        public bool Subledger { get; set; }
    }
}