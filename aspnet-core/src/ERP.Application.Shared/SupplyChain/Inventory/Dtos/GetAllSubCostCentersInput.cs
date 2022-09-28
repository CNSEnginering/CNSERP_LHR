using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class GetAllSubCostCentersInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CCIDFilter { get; set; }

		public int? MaxSUBCCIDFilter { get; set; }
		public int? MinSUBCCIDFilter { get; set; }

		public string SubCCNameFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}