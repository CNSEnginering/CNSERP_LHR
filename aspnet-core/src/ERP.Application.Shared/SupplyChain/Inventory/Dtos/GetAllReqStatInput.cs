using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class GetAllReqStatInput : PagedAndSortedResultRequestDto
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

		public double? MaxQtyFilter { get; set; }
		public double? MinQtyFilter { get; set; }

		public string RemarksFilter { get; set; }

		public double? MaxQIHFilter { get; set; }
		public double? MinQIHFilter { get; set; }

		public double? MaxPOQtyFilter { get; set; }
		public double? MinPOQtyFilter { get; set; }



    }
}