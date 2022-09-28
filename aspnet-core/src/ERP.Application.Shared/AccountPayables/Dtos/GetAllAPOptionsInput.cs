using Abp.Application.Services.Dto;
using System;

namespace ERP.AccountPayables.Dtos
{
    public class GetAllAPOptionsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string DEFBANKIDFilter { get; set; }

		public int? MaxDEFPAYCODEFilter { get; set; }
		public int? MinDEFPAYCODEFilter { get; set; }

		public string DEFVENCTRLACCFilter { get; set; }

		public string DEFCURRCODEFilter { get; set; }

		public string PAYTERMSFilter { get; set; }

		public DateTime? MaxAUDTDATEFilter { get; set; }
		public DateTime? MinAUDTDATEFilter { get; set; }

		public string AUDTUSERFilter { get; set; }


		 public string CurrencyRateIdFilter { get; set; }

		 		 public string BankBANKIDFilter { get; set; }

		 		 public string ChartofControlIdFilter { get; set; }

		 
    }
}