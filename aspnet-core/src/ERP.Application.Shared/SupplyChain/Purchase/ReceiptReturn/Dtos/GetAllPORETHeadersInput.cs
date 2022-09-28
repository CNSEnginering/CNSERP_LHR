using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Purchase.ReceiptReturn.Dtos
{
    public class GetAllPORETHeadersInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxLocIDFilter { get; set; }
		public int? MinLocIDFilter { get; set; }

		public int? MaxDocNoFilter { get; set; }
		public int? MinDocNoFilter { get; set; }

		public DateTime? MaxDocDateFilter { get; set; }
		public DateTime? MinDocDateFilter { get; set; }

		public string AccountIDFilter { get; set; }

		public int? MaxSubAccIDFilter { get; set; }
		public int? MinSubAccIDFilter { get; set; }

		public string NarrationFilter { get; set; }

		public string IGPNoFilter { get; set; }

		public string BillNoFilter { get; set; }

		public DateTime? MaxBillDateFilter { get; set; }
		public DateTime? MinBillDateFilter { get; set; }

		public double? MaxBillAmtFilter { get; set; }
		public double? MinBillAmtFilter { get; set; }

		public double? MaxTotalQtyFilter { get; set; }
		public double? MinTotalQtyFilter { get; set; }

		public double? MaxTotalAmtFilter { get; set; }
		public double? MinTotalAmtFilter { get; set; }

		public int PostedFilter { get; set; }

		public int? MaxLinkDetIDFilter { get; set; }
		public int? MinLinkDetIDFilter { get; set; }

		public int? MaxOrdNoFilter { get; set; }
		public int? MinOrdNoFilter { get; set; }

		public int? MaxRecDocNoFilter { get; set; }
		public int? MinRecDocNoFilter { get; set; }

		public double? MaxFreightFilter { get; set; }
		public double? MinFreightFilter { get; set; }

		public double? MaxAddExpFilter { get; set; }
		public double? MinAddExpFilter { get; set; }

		public int? MaxCCIDFilter { get; set; }
		public int? MinCCIDFilter { get; set; }

		public double? MaxAddDiscFilter { get; set; }
		public double? MinAddDiscFilter { get; set; }

		public double? MaxAddLeakFilter { get; set; }
		public double? MinAddLeakFilter { get; set; }

		public double? MaxAddFreightFilter { get; set; }
		public double? MinAddFreightFilter { get; set; }

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