using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.AllowanceSetup.Dtos
{
    public class GetAllAllowanceSetupInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxDocIDFilter { get; set; }
		public int? MinDocIDFilter { get; set; }

		public double? MaxFuelRateFilter { get; set; }
		public double? MinFuelRateFilter { get; set; }

		public double? MaxMilageRateFilter { get; set; }
		public double? MinMilageRateFilter { get; set; }

		public double? MaxRepairRateFilter { get; set; }
		public double? MinRepairRateFilter { get; set; }

		public int ActiveFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}