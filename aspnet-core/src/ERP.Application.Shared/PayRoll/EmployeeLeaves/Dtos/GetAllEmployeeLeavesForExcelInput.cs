using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.EmployeeLeaves.Dtos
{
    public class GetAllEmployeeLeavesForExcelInput
    {
		public string Filter { get; set; }

		public int? MaxEmployeeIDFilter { get; set; }
		public int? MinEmployeeIDFilter { get; set; }

        public int? MaxLeaveIDFilter { get; set; }
        public int? MinLeaveIDFilter { get; set; }
       // public string EmployeeNameFilter { get; set; }

        public int? MaxSalaryYearFilter { get; set; }
		public int? MinSalaryYearFilter { get; set; }

		public short? MaxSalaryMonthFilter { get; set; }
		public short? MinSalaryMonthFilter { get; set; }

		public DateTime? MaxStartDateFilter { get; set; }
		public DateTime? MinStartDateFilter { get; set; }

		public double? MaxLeaveTypeFilter { get; set; }
		public double? MinLeaveTypeFilter { get; set; }

        public double? MaxCasualFilter { get; set; }
        public double? MinCasualFilter { get; set; }

        public double? MaxSickFilter { get; set; }
        public double? MinSickFilter { get; set; }

        public double? MaxAnnualFilter { get; set; }
        public double? MinAnnualFilter { get; set; }

        public string PayTypeFilter { get; set; }
        public string RemarksFilter { get; set; }
        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}