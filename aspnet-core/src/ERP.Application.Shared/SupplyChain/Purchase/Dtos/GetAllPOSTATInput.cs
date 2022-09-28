using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Purchase.Dtos
{
    public class GetAllPOSTATInput : PagedAndSortedResultRequestDto
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

		public double? MaxRateFilter { get; set; }
		public double? MinRateFilter { get; set; }

		public double? MaxAmountFilter { get; set; }
		public double? MinAmountFilter { get; set; }

		public string RemarksFilter { get; set; }

		public double? MaxReceivedFilter { get; set; }
		public double? MinReceivedFilter { get; set; }

		public double? MaxReturnedFilter { get; set; }
		public double? MinReturnedFilter { get; set; }

		public int? MaxReqNoFilter { get; set; }
		public int? MinReqNoFilter { get; set; }

		public DateTime? MaxDocDateFilter { get; set; }
		public DateTime? MinDocDateFilter { get; set; }



    }
}