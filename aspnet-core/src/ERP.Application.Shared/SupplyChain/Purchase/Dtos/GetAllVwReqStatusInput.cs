using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Purchase.Dtos
{
    public class GetAllVwReqStatusInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxLocIDFilter { get; set; }
		public int? MinLocIDFilter { get; set; }

		public int? MaxDocNoFilter { get; set; }
		public int? MinDocNoFilter { get; set; }

		public DateTime? MaxDocDateFilter { get; set; }
		public DateTime? MinDocDateFilter { get; set; }

		public string NarrationFilter { get; set; }

		public string ItemIDFilter { get; set; }

		public string DescpFilter { get; set; }

		public double? MaxreqqtyFilter { get; set; }
		public double? MinreqqtyFilter { get; set; }

		public double? MaxpoqtyFilter { get; set; }
		public double? MinpoqtyFilter { get; set; }

		public double? MaxrecqtyFilter { get; set; }
		public double? MinrecqtyFilter { get; set; }

		public DateTime? MaxpodateFilter { get; set; }
		public DateTime? MinpodateFilter { get; set; }

		public DateTime? MaxrecdateFilter { get; set; }
		public DateTime? MinrecdateFilter { get; set; }

		public string party_nameFilter { get; set; }

		public string locationFilter { get; set; }

		public string rec_narrationFilter { get; set; }

		public string OrdNoFilter { get; set; }

		public int? MaxrecdocnoFilter { get; set; }
		public int? MinrecdocnoFilter { get; set; }

		public int? MaxpodocnoFilter { get; set; }
		public int? MinpodocnoFilter { get; set; }

		public int? MaxSUBCCIDFilter { get; set; }
		public int? MinSUBCCIDFilter { get; set; }



    }
}