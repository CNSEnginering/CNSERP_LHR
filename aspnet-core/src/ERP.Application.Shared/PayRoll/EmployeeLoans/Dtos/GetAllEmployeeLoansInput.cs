using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.EmployeeLoans.Dtos
{
    public class GetAllEmployeeLoansInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxEmployeeIDFilter { get; set; }
		public int? MinEmployeeIDFilter { get; set; }

		public int? MaxLoanIDFilter { get; set; }
		public int? MinLoanIDFilter { get; set; }

		public DateTime? MaxLoanDateFilter { get; set; }
		public DateTime? MinLoanDateFilter { get; set; }

		public int? MaxLoanTypeIDFilter { get; set; }
		public int? MinLoanTypeIDFilter { get; set; }

		public double? MaxAmountFilter { get; set; }
		public double? MinAmountFilter { get; set; }

		public short? MaxNOIFilter { get; set; }
		public short? MinNOIFilter { get; set; }

		public double? MaxInsAmtFilter { get; set; }
		public double? MinInsAmtFilter { get; set; }

		public string RemarksFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}