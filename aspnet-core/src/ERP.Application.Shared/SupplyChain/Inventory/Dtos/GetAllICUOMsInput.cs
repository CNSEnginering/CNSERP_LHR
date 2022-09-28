using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class GetAllICUOMsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string UnitFilter { get; set; }

		public string UNITDESCFilter { get; set; }

		public double? MaxConverFilter { get; set; }
		public double? MinConverFilter { get; set; }

		public int ActiveFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }



    }
}