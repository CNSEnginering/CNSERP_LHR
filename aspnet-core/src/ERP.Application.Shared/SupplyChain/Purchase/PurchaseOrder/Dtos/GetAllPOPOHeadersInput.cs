using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Purchase.PurchaseOrder.Dtos
{
    public class GetAllPOPOHeadersInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxLocIDFilter { get; set; }
		public int? MinLocIDFilter { get; set; }

		public int? MaxDocNoFilter { get; set; }
		public int? MinDocNoFilter { get; set; }

		public DateTime? MaxDocDateFilter { get; set; }
		public DateTime? MinDocDateFilter { get; set; }

		public DateTime? MaxArrivalDateFilter { get; set; }
		public DateTime? MinArrivalDateFilter { get; set; }

		public int? MaxReqNoFilter { get; set; }
		public int? MinReqNoFilter { get; set; }

		public string AccountIDFilter { get; set; }

		public int? MaxSubAccIDFilter { get; set; }
		public int? MinSubAccIDFilter { get; set; }

		public double? MaxTotalQtyFilter { get; set; }
		public double? MinTotalQtyFilter { get; set; }

		public double? MaxTotalAmtFilter { get; set; }
		public double? MinTotalAmtFilter { get; set; }

		public string OrdNoFilter { get; set; }

		public string CCIDFilter { get; set; }

		public string NarrationFilter { get; set; }

		public int? MaxWHTermIDFilter { get; set; }
		public int? MinWHTermIDFilter { get; set; }

		public double? MaxWHRateFilter { get; set; }
		public double? MinWHRateFilter { get; set; }

		public string TaxAuthFilter { get; set; }

		public int? MaxTaxClassFilter { get; set; }
		public int? MinTaxClassFilter { get; set; }

		public double? MaxTaxRateFilter { get; set; }
		public double? MinTaxRateFilter { get; set; }

		public double? MaxTaxAmountFilter { get; set; }
		public double? MinTaxAmountFilter { get; set; }

		public int onHoldFilter { get; set; }

		public int ActiveFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}