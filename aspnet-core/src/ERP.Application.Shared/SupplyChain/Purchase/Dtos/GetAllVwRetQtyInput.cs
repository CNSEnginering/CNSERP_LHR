using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Purchase.Dtos
{
    public class GetAllVwRetQtyInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxLocIDFilter { get; set; }
		public int? MinLocIDFilter { get; set; }

		public int? MaxDocNoFilter { get; set; }
		public int? MinDocNoFilter { get; set; }

		public string ItemIDFilter { get; set; }

		public string UnitFilter { get; set; }

		public double? MaxConverFilter { get; set; }
		public double? MinConverFilter { get; set; }

		public string RemarksFilter { get; set; }

		public double? MaxRateFilter { get; set; }
		public double? MinRateFilter { get; set; }

		public double? MaxQtyFilter { get; set; }
		public double? MinQtyFilter { get; set; }

		public double? MaxAmountFilter { get; set; }
		public double? MinAmountFilter { get; set; }

		public double? MaxqtypFilter { get; set; }
		public double? MinqtypFilter { get; set; }

		public string descpFilter { get; set; }



    }
}