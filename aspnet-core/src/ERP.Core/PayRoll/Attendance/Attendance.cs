using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.Attendance
{
	[Table("tempAttendance")]
    public class Attendance : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int EmployeeID { get; set; }
		
		public virtual string EmployeeName { get; set; }
		
		public virtual DateTime? AttendanceDate { get; set; }
		
		public virtual int? ShiftID { get; set; }
		
		public virtual DateTime? TimeIn { get; set; }
		
		public virtual DateTime? TimeOut { get; set; }
		
		public virtual DateTime? BreakOut { get; set; }
		
		public virtual DateTime? BreakIn { get; set; }
		
		public virtual double? TotalHrs { get; set; }
		
		public virtual string LeaveType { get; set; }
		
		public virtual double? LeaveHours { get; set; }
		
		public virtual double? OTHours { get; set; }
		
		public virtual string Reason { get; set; }
		

    }
}