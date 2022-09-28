using Abp.Application.Services.Dto;
using System;

namespace ERP.AccountReceivables.RouteInvoices.Dtos
{
    public class GetAllARINVDInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxDetIDFilter { get; set; }
		public int? MinDetIDFilter { get; set; }

		public string AccountIDFilter { get; set; }

		public int? MaxSubAccIDFilter { get; set; }
		public int? MinSubAccIDFilter { get; set; }

		public int? MaxDocNoFilter { get; set; }
		public int? MinDocNoFilter { get; set; }

		public string InvNumberFilter { get; set; }

		public double? MaxInvAmountFilter { get; set; }
		public double? MinInvAmountFilter { get; set; }

		public string TaxAmountFilter { get; set; }

		public string TaxAuthFilter { get; set; }

		public int? MaxTaxClassFilter { get; set; }
		public int? MinTaxClassFilter { get; set; }

		public double? MaxTaxRateFilter { get; set; }
		public double? MinTaxRateFilter { get; set; }

		public double? MaxRecpAmountFilter { get; set; }
		public double? MinRecpAmountFilter { get; set; }

		public string ChequeNoFilter { get; set; }

		public int AdjustFilter { get; set; }

		public string NarrationFilter { get; set; }



    }
}