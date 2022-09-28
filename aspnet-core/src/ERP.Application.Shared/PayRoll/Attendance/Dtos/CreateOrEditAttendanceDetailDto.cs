
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.Attendance.Dtos
{
    public class CreateOrEditAttendanceDetailDto : EntityDto<int?>
    {

		[Required]
		public int EmployeeID { get; set; }
		public int TenantId { get; set; }
		
		
		//[Required]
		public string EmployeeName { get; set; }
		
		
		//[Required]
		public DateTime? AttendanceDate { get; set; }
		
		
		public int? ShiftID { get; set; }
        public int? DetID { get; set; }


        
        public DateTime? TimeIn { get; set; }

        
        public DateTime? TimeOut { get; set; }
		
		
		public DateTime? BreakOut { get; set; }
		
		
		public DateTime? BreakIn { get; set; }
		
		
		public decimal? TotalHrs { get; set; }

		public string Reason { get; set; }



	}
}