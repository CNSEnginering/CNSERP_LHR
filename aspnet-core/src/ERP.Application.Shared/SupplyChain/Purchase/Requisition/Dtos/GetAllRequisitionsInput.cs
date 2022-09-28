using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Purchase.Requisition.Dtos
{
    public class GetAllRequisitionsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxLocIDFilter { get; set; }
		public int? MinLocIDFilter { get; set; }

		public int? MaxDocNoFilter { get; set; }
		public int? MinDocNoFilter { get; set; }

		public DateTime? MaxDocDateFilter { get; set; }
		public DateTime? MinDocDateFilter { get; set; }

		public DateTime? MaxExpArrivalDateFilter { get; set; }
		public DateTime? MinExpArrivalDateFilter { get; set; }

		public int? MaxOrdNoFilter { get; set; }
		public int? MinOrdNoFilter { get; set; }

		public string CCIDFilter { get; set; }

		public string NarrationFilter { get; set; }

		public double? MaxTotalQtyFilter { get; set; }
		public double? MinTotalQtyFilter { get; set; }

		public DateTime? MaxArrivalDateFilter { get; set; }
		public DateTime? MinArrivalDateFilter { get; set; }

		public int? MaxReqNoFilter { get; set; }
		public int? MinReqNoFilter { get; set; }

		public string AuditUserFilter { get; set; }

		public DateTime? MaxAuditTimeFilter { get; set; }
		public DateTime? MinAuditTimeFilter { get; set; }

		public decimal? MaxSysDateFilter { get; set; }
		public decimal? MinSysDateFilter { get; set; }

		public int? MaxDbIDFilter { get; set; }
		public int? MinDbIDFilter { get; set; }

		public int CompletedFilter { get; set; }

		public int ActiveFilter { get; set; }

		public int HoldFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}