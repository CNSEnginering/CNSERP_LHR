
using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.Attendance.Dtos
{
    public class AttendanceDetailDto : EntityDto
    {
		public int EmployeeID { get; set; }

		public string EmployeeName { get; set; }

		public DateTime? AttendanceDate { get; set; }

		public int? ShiftID { get; set; }
		public int? DetID { get; set; }

		public DateTime? TimeIn { get; set; }

		public DateTime? TimeOut { get; set; }

		public DateTime? BreakOut { get; set; }

		public DateTime? BreakIn { get; set; }

		public decimal? TotalHrs { get; set; }
        public virtual string LeaveType { get; set; }

        public virtual decimal? LeaveHours { get; set; }
        public virtual decimal? GraceHours { get; set; }

        public virtual decimal? OTHours { get; set; }

        public bool include { get; set; }

    }
}