using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class GetAllReorderLevelsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public decimal? MaxMinLevelFilter { get; set; }
		public decimal? MinMinLevelFilter { get; set; }

		public decimal? MaxMaxLevelFilter { get; set; }
		public decimal? MinMaxLevelFilter { get; set; }

		public decimal? MaxOrdLevelFilter { get; set; }
		public decimal? MinOrdLevelFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public int? MaxLocIdFilter { get; set; }
		public int? MinLocIdFilter { get; set; }

		public string ItemIdFilter { get; set; }



    }
}