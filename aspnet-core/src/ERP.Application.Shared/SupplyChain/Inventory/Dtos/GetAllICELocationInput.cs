using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class GetAllICELocationInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxIDFilter { get; set; }
		public int? MinIDFilter { get; set; }

		public int? MaxTenantIDFilter { get; set; }
		public int? MinTenantIDFilter { get; set; }

		public string LocationTitleFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string ApprovedByFilter { get; set; }

		public DateTime? MaxApprovedDateFilter { get; set; }
		public DateTime? MinApprovedDateFilter { get; set; }



    }
}