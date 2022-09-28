using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.Allowances.Dtos
{
    public class GetAllAllowancesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxDocIDFilter { get; set; }
		public int? MinDocIDFilter { get; set; }

		public DateTime? MaxDocdateFilter { get; set; }
		public DateTime? MinDocdateFilter { get; set; }

		public short? MaxDocMonthFilter { get; set; }
		public short? MinDocMonthFilter { get; set; }

		public int? MaxDocYearFilter { get; set; }
		public int? MinDocYearFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}