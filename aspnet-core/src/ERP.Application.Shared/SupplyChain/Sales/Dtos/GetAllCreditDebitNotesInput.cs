using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Sales.Dtos
{
    public class GetAllCreditDebitNotesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxLocIDFilter { get; set; }
		public int? MinLocIDFilter { get; set; }

		public int? MaxDocNoFilter { get; set; }
		public int? MinDocNoFilter { get; set; }

		public DateTime? MaxDocDateFilter { get; set; }
		public DateTime? MinDocDateFilter { get; set; }

		public DateTime? MaxPostingDateFilter { get; set; }
		public DateTime? MinPostingDateFilter { get; set; }

		public DateTime? MaxPaymentDateFilter { get; set; }
		public DateTime? MinPaymentDateFilter { get; set; }

		public short? MaxTypeIDFilter { get; set; }
		public short? MinTypeIDFilter { get; set; }

		public string AccountIDFilter { get; set; }

		public int? MaxSubAccIDFilter { get; set; }
		public int? MinSubAccIDFilter { get; set; }

		public string ReasonFilter { get; set; }

		public string NarrationFilter { get; set; }

		public string OGPFilter { get; set; }

		public double? MaxTotalQtyFilter { get; set; }
		public double? MinTotalQtyFilter { get; set; }

		public double? MaxTotAmtFilter { get; set; }
		public double? MinTotAmtFilter { get; set; }

		public int PostedFilter { get; set; }

		public int? MaxLinkDetIDFilter { get; set; }
		public int? MinLinkDetIDFilter { get; set; }

		public int ActiveFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}