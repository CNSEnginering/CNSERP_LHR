using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.Holidays.Dtos
{
    public class GetAllHolidaysForExcelInput
    {
		public string Filter { get; set; }

		public int? MaxHolidayIDFilter { get; set; }
		public int? MinHolidayIDFilter { get; set; }

		public DateTime? MaxHolidayDateFilter { get; set; }
		public DateTime? MinHolidayDateFilter { get; set; }

		public string HolidayNameFilter { get; set; }

		public int ActiveFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}