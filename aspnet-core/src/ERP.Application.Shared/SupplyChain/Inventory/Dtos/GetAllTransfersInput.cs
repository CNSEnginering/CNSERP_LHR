using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class GetAllTransfersInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxFromLocIDFilter { get; set; }
		public int? MinFromLocIDFilter { get; set; }

		public int? MaxDocNoFilter { get; set; }
		public int? MinDocNoFilter { get; set; }

		public DateTime? MaxDocDateFilter { get; set; }
		public DateTime? MinDocDateFilter { get; set; }

		public string NarrationFilter { get; set; }

		public int? MaxToLocIDFilter { get; set; }
		public int? MinToLocIDFilter { get; set; }

		public decimal? MaxTotalQtyFilter { get; set; }
		public decimal? MinTotalQtyFilter { get; set; }

		public decimal? MaxTotalAmtFilter { get; set; }
		public decimal? MinTotalAmtFilter { get; set; }

		public int PostedFilter { get; set; }

        public string PostedByFilter { get; set; }
        public int ActiveFilter { get; set; }
        public DateTime? MaxPostedDateFilter { get; set; }

        public DateTime? MinPostedDateFilter { get; set; }

        public int? MaxLinkDetIDFilter { get; set; }
		public int? MinLinkDetIDFilter { get; set; }

		public string OrdNoFilter { get; set; }

		public int HOLDFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}