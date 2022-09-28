using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Sales.SaleReturn.Dtos
{
    public class GetAllOERETHeadersInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxLocIDFilter { get; set; }
		public int? MinLocIDFilter { get; set; }

		public int? MaxDocNoFilter { get; set; }
		public int? MinDocNoFilter { get; set; }

		public DateTime? MaxDocDateFilter { get; set; }
		public DateTime? MinDocDateFilter { get; set; }

		public DateTime? MaxPaymentDateFilter { get; set; }
		public DateTime? MinPaymentDateFilter { get; set; }

		public string TypeIDFilter { get; set; }

		public int? MaxCustIDFilter { get; set; }
		public int? MinCustIDFilter { get; set; }

		public string PriceListFilter { get; set; }

		public string NarrationFilter { get; set; }

		public string OGPFilter { get; set; }

		public double? MaxTotalQtyFilter { get; set; }
		public double? MinTotalQtyFilter { get; set; }

		public double? MaxAmountFilter { get; set; }
		public double? MinAmountFilter { get; set; }

		public double? MaxTaxFilter { get; set; }
		public double? MinTaxFilter { get; set; }

		public double? MaxAddTaxFilter { get; set; }
		public double? MinAddTaxFilter { get; set; }

		public double? MaxDiscFilter { get; set; }
		public double? MinDiscFilter { get; set; }

		public double? MaxTradeDiscFilter { get; set; }
		public double? MinTradeDiscFilter { get; set; }

		public double? MaxMarginFilter { get; set; }
		public double? MinMarginFilter { get; set; }

		public double? MaxFreightFilter { get; set; }
		public double? MinFreightFilter { get; set; }

		public string OrdNoFilter { get; set; }

		public double? MaxTotAmtFilter { get; set; }
		public double? MinTotAmtFilter { get; set; }

		public int PostedFilter { get; set; }

		public int? MaxLinkDetIDFilter { get; set; }
		public int? MinLinkDetIDFilter { get; set; }

		public int ActiveFilter { get; set; }

		public int? MaxSDocNoFilter { get; set; }
		public int? MinSDocNoFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}