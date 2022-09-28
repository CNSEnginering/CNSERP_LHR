using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class GetAllICLocationsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxLocIDFilter { get; set; }
		public int? MinLocIDFilter { get; set; }

		public string LocNameFilter { get; set; }

		public string LocShortFilter { get; set; }

		public string AddressFilter { get; set; }

		public string CityFilter { get; set; }

		public int AllowRecFilter { get; set; }

		public int AllowNegFilter { get; set; }

		public int ActiveFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }



    }
}