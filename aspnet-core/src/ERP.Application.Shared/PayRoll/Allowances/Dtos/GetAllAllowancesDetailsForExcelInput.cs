using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.Allowances.Dtos
{
    public class GetAllAllowancesDetailsForExcelInput
    {
		public string Filter { get; set; }

		public int? MaxDetIDFilter { get; set; }
		public int? MinDetIDFilter { get; set; }

		public int? MaxTenantIDFilter { get; set; }
		public int? MinTenantIDFilter { get; set; }

		public int? MaxEmployeeIDFilter { get; set; }
		public int? MinEmployeeIDFilter { get; set; }

		public short? MaxAllowanceTypeFilter { get; set; }
		public short? MinAllowanceTypeFilter { get; set; }

		public double? MaxAllowanceAmtFilter { get; set; }
		public double? MinAllowanceAmtFilter { get; set; }

		public double? MaxAllowanceQtyFilter { get; set; }
		public double? MinAllowanceQtyFilter { get; set; }

		public double? MaxAmountFilter { get; set; }
		public double? MinAmountFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}