using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.WorkOrder.Dtos
{
    public class GetAllICWODetailsInput : PagedAndSortedResultRequestDto
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

		public double? MaxConverFilter { get; set; }
		public double? MinConverFilter { get; set; }

		public double? MaxQtyFilter { get; set; }
		public double? MinQtyFilter { get; set; }

		public double? MaxCostFilter { get; set; }
		public double? MinCostFilter { get; set; }

		public double? MaxAmountFilter { get; set; }
		public double? MinAmountFilter { get; set; }

		public string RemarksFilter { get; set; }



    }
}