using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.EmployeeDeductions.Dtos
{
    public class GetAllEmployeeDeductionsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxDeductionIDFilter { get; set; }
		public int? MinDeductionIDFilter { get; set; }

		public int? MaxEmployeeIDFilter { get; set; }
		public int? MinEmployeeIDFilter { get; set; }

        public string EmployeeNameFilter { get; set; }

        public short? MaxSalaryYearFilter { get; set; }
		public short? MinSalaryYearFilter { get; set; }

		public short? MaxSalaryMonthFilter { get; set; }
		public short? MinSalaryMonthFilter { get; set; }

		public DateTime? MaxDeductionDateFilter { get; set; }
		public DateTime? MinDeductionDateFilter { get; set; }

		public double? MaxAmountFilter { get; set; }
		public double? MinAmountFilter { get; set; }

		public int ActiveFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}