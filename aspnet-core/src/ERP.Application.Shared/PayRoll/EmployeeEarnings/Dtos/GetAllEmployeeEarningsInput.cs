using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.EmployeeEarnings.Dtos
{
    public class GetAllEmployeeEarningsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxEarningIDFilter { get; set; }
		public int? MinEarningIDFilter { get; set; }

		public int? MaxEmployeeIDFilter { get; set; }
		public int? MinEmployeeIDFilter { get; set; }

        public string EmployeeNameFilter { get; set; }

        public short? MaxSalaryYearFilter { get; set; }
		public short? MinSalaryYearFilter { get; set; }

		public short? MaxSalaryMonthFilter { get; set; }
		public short? MinSalaryMonthFilter { get; set; }

		public DateTime? MaxEarningDateFilter { get; set; }
		public DateTime? MinEarningDateFilter { get; set; }

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