using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class GetAllGatePassesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public short? MaxTypeIDFilter { get; set; }
		public short? MinTypeIDFilter { get; set; }

		public int? MaxDocNoFilter { get; set; }
		public int? MinDocNoFilter { get; set; }

		public DateTime? MaxDocDateFilter { get; set; }
		public DateTime? MinDocDateFilter { get; set; }

		public string AccountIDFilter { get; set; }

		public int? MaxPartyIDFilter { get; set; }
		public int? MinPartyIDFilter { get; set; }

		public string NarrationFilter { get; set; }

		public short? MaxGPTypeFilter { get; set; }
		public short? MinGPTypeFilter { get; set; }

		public string DriverNameFilter { get; set; }

		public string VehicleNoFilter { get; set; }

		public int? MaxGPDocNoFilter { get; set; }
		public int? MinGPDocNoFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}