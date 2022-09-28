using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetAllAccountSubLedgersInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string AccountIDFilter { get; set; }
		public int? MinSubAccIDFilter { get; set; }
		public int? MaxSubAccIDFilter { get; set; }

		public string SubAccNameFilter { get; set; }

		
		public string CityFilter { get; set; }

		public string PhoneFilter { get; set; }

		public string ContactFilter { get; set; }

		public string RegNoFilter { get; set; }

		public string TAXAUTHFilter { get; set; }
        
		 public string ChartofControlAccountNameFilter { get; set; }

		 public string TaxAuthorityTAXAUTHDESCFilter { get; set; }
        public string SlType { get; set; }


    }
}