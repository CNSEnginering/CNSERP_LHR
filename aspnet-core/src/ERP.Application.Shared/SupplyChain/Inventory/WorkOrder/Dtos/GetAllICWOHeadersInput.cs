using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.WorkOrder.Dtos
{
    public class GetAllICWOHeadersInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxLocIDFilter { get; set; }
		public int? MinLocIDFilter { get; set; }

		public int? MaxDocNoFilter { get; set; }
		public int? MinDocNoFilter { get; set; }

		public DateTime? MaxDocDateFilter { get; set; }
		public DateTime? MinDocDateFilter { get; set; }

		public string CCIDFilter { get; set; }

		public string NarrationFilter { get; set; }

		public double? MaxTotalQtyFilter { get; set; }
		public double? MinTotalQtyFilter { get; set; }

		public int ActiveFilter { get; set; }
        public int PostedFilter { get; set; }

        public int ApprovedFilter { get; set; }

		public string ApprovedByFilter { get; set; }

		public DateTime? MaxApprovedDateFilter { get; set; }
		public DateTime? MinApprovedDateFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}