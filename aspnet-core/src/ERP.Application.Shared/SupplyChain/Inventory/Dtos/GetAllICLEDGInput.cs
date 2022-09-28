using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class GetAllICLEDGInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public DateTime? MaxDocDateFilter { get; set; }
		public DateTime? MinDocDateFilter { get; set; }

		public int? MaxDocTypeFilter { get; set; }
		public int? MinDocTypeFilter { get; set; }

		public string DocDescFilter { get; set; }

		public int? MaxDocNoFilter { get; set; }
		public int? MinDocNoFilter { get; set; }

		public int? MaxLocIDFilter { get; set; }
		public int? MinLocIDFilter { get; set; }

		public string ItemIDFilter { get; set; }

		public string srnoFilter { get; set; }

		public string UNITFilter { get; set; }

		public double? MaxConverFilter { get; set; }
		public double? MinConverFilter { get; set; }

		public double? MaxQtyFilter { get; set; }
		public double? MinQtyFilter { get; set; }

		public double? MaxRateFilter { get; set; }
		public double? MinRateFilter { get; set; }

		public double? MaxAmountFilter { get; set; }
		public double? MinAmountFilter { get; set; }

		public string DescpFilter { get; set; }

		public string TableNameFilter { get; set; }

		public int? MaxPKIDFilter { get; set; }
		public int? MinPKIDFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }

		public string JobNoFilter { get; set; }



    }
}