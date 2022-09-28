using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.Adjustments.Dtos
{
    public class GetAllAdjHInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxTenantIDFilter { get; set; }
		public int? MinTenantIDFilter { get; set; }

		public int? MaxDocTypeFilter { get; set; }
		public int? MinDocTypeFilter { get; set; }

		public int? MaxTypeIDFilter { get; set; }
		public int? MinTypeIDFilter { get; set; }

		public int? MaxDocIDFilter { get; set; }
		public int? MinDocIDFilter { get; set; }

		public DateTime? MaxDocdateFilter { get; set; }
		public DateTime? MinDocdateFilter { get; set; }

		public short? MaxSalaryYearFilter { get; set; }
		public short? MinSalaryYearFilter { get; set; }

		public short? MaxSalaryMonthFilter { get; set; }
		public short? MinSalaryMonthFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }




    }
}