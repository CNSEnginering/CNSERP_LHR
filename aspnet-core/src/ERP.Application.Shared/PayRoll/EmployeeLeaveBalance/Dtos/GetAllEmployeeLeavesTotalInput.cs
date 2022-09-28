using Abp.Application.Services.Dto;
using System;

namespace ERP.Payroll.EmployeeLeaveBalance.Dtos
{
    public class GetAllEmployeeLeavesTotalInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxSalaryYearFilter { get; set; }
		public int? MinSalaryYearFilter { get; set; }

		public int? MaxEmployeeIDFilter { get; set; }
		public int? MinEmployeeIDFilter { get; set; }

		public double? MaxLeavesFilter { get; set; }
		public double? MinLeavesFilter { get; set; }

		public double? MaxCasualFilter { get; set; }
		public double? MinCasualFilter { get; set; }

		public double? MaxSickFilter { get; set; }
		public double? MinSickFilter { get; set; }

		public double? MaxAnnualFilter { get; set; }
		public double? MinAnnualFilter { get; set; }

		public double? MaxCPLFilter { get; set; }
		public double? MinCPLFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }




    }
}