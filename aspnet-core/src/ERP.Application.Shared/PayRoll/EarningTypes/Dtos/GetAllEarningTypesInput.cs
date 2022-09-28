using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.EarningTypes.Dtos
{
    public class GetAllEarningTypesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxTypeIDFilter { get; set; }
		public int? MinTypeIDFilter { get; set; }

		public string TypeDescFilter { get; set; }

		public int ActiveFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}