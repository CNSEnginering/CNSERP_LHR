using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class GetAllTransactionTypesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string DescriptionFilter { get; set; }

		public int ActiveFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string TypeIdFilter { get; set; }



    }
}