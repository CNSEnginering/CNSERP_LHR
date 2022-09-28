using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetAllCurrencyRatesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CMPIDFilter { get; set; }

		public string CURIDFilter { get; set; }

		public DateTime? MaxAUDTDATEFilter { get; set; }
		public DateTime? MinAUDTDATEFilter { get; set; }

		public string AUDTUSERFilter { get; set; }

		public string CURNAMEFilter { get; set; }

		public string SYMBOLFilter { get; set; }

		public DateTime? MaxRATEDATEFilter { get; set; }
		public DateTime? MinRATEDATEFilter { get; set; }

		public double? MaxCURRATEFilter { get; set; }
		public double? MinCURRATEFilter { get; set; }


		 public string CompanyProfileCompanyNameFilter { get; set; }

		 
    }
}