using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.Opening.Dtos
{
    public class GetAllICOPNDetailsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxDetIDFilter { get; set; }
		public int? MinDetIDFilter { get; set; }

		public int? MaxLocIDFilter { get; set; }
		public int? MinLocIDFilter { get; set; }

		public int? MaxDocNoFilter { get; set; }
		public int? MinDocNoFilter { get; set; }

		public string ItemIDFilter { get; set; }

		public string UnitFilter { get; set; }

		public decimal? MaxConverFilter { get; set; }
		public decimal? MinConverFilter { get; set; }

		public decimal? MaxQtyFilter { get; set; }
		public decimal? MinQtyFilter { get; set; }

		public decimal? MaxRateFilter { get; set; }
		public decimal? MinRateFilter { get; set; }

		public decimal? MaxAmountFilter { get; set; }
		public decimal? MinAmountFilter { get; set; }

		public string CommentsFilter { get; set; }

		public int ActiveFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }



    }
}