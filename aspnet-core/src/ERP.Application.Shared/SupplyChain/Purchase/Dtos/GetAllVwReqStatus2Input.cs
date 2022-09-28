using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Purchase.Dtos
{
    public class GetAllVwReqStatus2Input : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxLocidFilter { get; set; }
		public int? MinLocidFilter { get; set; }

		public int? MaxDocnoFilter { get; set; }
		public int? MinDocnoFilter { get; set; }

		public string ItemIDFilter { get; set; }

		public string DescpFilter { get; set; }

		public string UnitFilter { get; set; }

		public double? MaxConverFilter { get; set; }
		public double? MinConverFilter { get; set; }

		public double? MaxReqQtyFilter { get; set; }
		public double? MinReqQtyFilter { get; set; }

		public double? MaxQIHFilter { get; set; }
		public double? MinQIHFilter { get; set; }

		public double? MaxPOQtyFilter { get; set; }
		public double? MinPOQtyFilter { get; set; }

		public double? MaxReceivedFilter { get; set; }
		public double? MinReceivedFilter { get; set; }

		public double? MaxReturnedFilter { get; set; }
		public double? MinReturnedFilter { get; set; }

		public double? MaxQtyPFilter { get; set; }
		public double? MinQtyPFilter { get; set; }



    }
}