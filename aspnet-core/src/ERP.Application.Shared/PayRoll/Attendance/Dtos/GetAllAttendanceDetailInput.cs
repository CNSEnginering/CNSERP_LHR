using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.Attendance.Dtos
{
    public class GetAllAttendanceDetailInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxEmployeeIDFilter { get; set; }
		public int? MinEmployeeIDFilter { get; set; }

		public string EmployeeNameFilter { get; set; }

		public DateTime? MaxAttendanceDateFilter { get; set; }
		public DateTime? MinAttendanceDateFilter { get; set; }

		public int? MaxShiftIDFilter { get; set; }
		public int? MinShiftIDFilter { get; set; }

        public int? MaxDetIDFilter { get; set; }
        public int? MinDetIDFilter { get; set; }

        public DateTime? MaxTimeInFilter { get; set; }
		public DateTime? MinTimeInFilter { get; set; }

		public DateTime? MaxTimeOutFilter { get; set; }
		public DateTime? MinTimeOutFilter { get; set; }

		public DateTime? MaxBreakOutFilter { get; set; }
		public DateTime? MinBreakOutFilter { get; set; }

		public DateTime? MaxBreakInFilter { get; set; }
		public DateTime? MinBreakInFilter { get; set; }

		public decimal? MaxTotalHrsFilter { get; set; }
		public decimal? MinTotalHrsFilter { get; set; }



    }
}