using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class GetAllItemPricingsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		//public string PriceListFilter { get; set; }

		//public string ItemIDFilter { get; set; }

		//public string priceTypeFilter { get; set; }

		public decimal? MaxPriceFilter { get; set; }
		public decimal? MinPriceFilter { get; set; }

		public double? MaxDiscValueFilter { get; set; }
		public double? MinDiscValueFilter { get; set; }

		public decimal? MaxNetPriceFilter { get; set; }
		public decimal? MinNetPriceFilter { get; set; }

		public short? MaxActiveFilter { get; set; }
		public short? MinActiveFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}