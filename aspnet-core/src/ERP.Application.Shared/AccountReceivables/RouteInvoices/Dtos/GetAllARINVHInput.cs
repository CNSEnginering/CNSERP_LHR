using Abp.Application.Services.Dto;
using System;

namespace ERP.AccountReceivables.RouteInvoices.Dtos
{
    public class GetAllARINVHInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxDocNoFilter { get; set; }
		public int? MinDocNoFilter { get; set; }

		public DateTime? MaxDocDateFilter { get; set; }
		public DateTime? MinDocDateFilter { get; set; }

		public DateTime? MaxInvDateFilter { get; set; }
		public DateTime? MinInvDateFilter { get; set; }

		public int? MaxLocIDFilter { get; set; }
		public int? MinLocIDFilter { get; set; }

		public int? MaxRoutIDFilter { get; set; }
		public int? MinRoutIDFilter { get; set; }

		public string RefNoFilter { get; set; }

		public string SaleTypeIDFilter { get; set; }

		public string PaymentOptionFilter { get; set; }

		public string NarrationFilter { get; set; }

		public string BankIDFilter { get; set; }

		public string AccountIDFilter { get; set; }

		public int? MaxConfigIDFilter { get; set; }
		public int? MinConfigIDFilter { get; set; }

		public string ChequeNoFilter { get; set; }

		public int? MaxLinkDetIDFilter { get; set; }
		public int? MinLinkDetIDFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }

		public int PostedFilter { get; set; }

		public string PostedByFilter { get; set; }

		public DateTime? MaxPostedDateFilter { get; set; }
		public DateTime? MinPostedDateFilter { get; set; }

		public string TaxAuthFilter { get; set; }

		public int? MaxTaxClassFilter { get; set; }
		public int? MinTaxClassFilter { get; set; }

		public double? MaxTaxRateFilter { get; set; }
		public double? MinTaxRateFilter { get; set; }

		public string TaxAccIDFilter { get; set; }

		public double? MaxTaxAmountFilter { get; set; }
		public double? MinTaxAmountFilter { get; set; }

		public double? MaxInvAmountFilter { get; set; }
		public double? MinInvAmountFilter { get; set; }



    }
}