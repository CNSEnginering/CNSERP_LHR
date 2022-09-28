
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.Attendance.Dtos
{
    public class CreateOrEditAttendanceDto : EntityDto<int?>
    {

		[Required]
		public int EmployeeID { get; set; }
		
		
		public string EmployeeName { get; set; }
		
		
		public DateTime? AttendanceDate { get; set; }
		
		
		public int? ShiftID { get; set; }
		
		
		public DateTime? TimeIn { get; set; }
		
		
		public DateTime? TimeOut { get; set; }
		
		
		public DateTime? BreakOut { get; set; }
		
		
		public DateTime? BreakIn { get; set; }
		
		
		public double? TotalHrs { get; set; }
		
		
		public string LeaveType { get; set; }
		
		
		public double? LeaveHours { get; set; }
		
		
		public double? OTHours { get; set; }
		
		
		public string Reason { get; set; }
		
		

    }
}