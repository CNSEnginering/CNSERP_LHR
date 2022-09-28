using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class GetAllInventoryGlLinksInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxLocIDFilter { get; set; }
		public int? MinLocIDFilter { get; set; }

		//public string SegIDFilter { get; set; }

		//public string AccRecFilter { get; set; }

		//public string AccRetFilter { get; set; }

		//public string AccAdjFilter { get; set; }

		//public string AccCGSFilter { get; set; }

		//public string AccWIPFilter { get; set; }

		//public string AudtUserFilter { get; set; }
        //public string segNameFilter { get; set; }
       // public string locNameFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		//public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}