
using System;
using Abp.Application.Services.Dto;

namespace ERP.Payroll.EmployeeLeaveBalance.Dtos
{
    public class EmployeeLeavesTotalDto : EntityDto
    {
		public int SalaryYear { get; set; }

		public int EmployeeID { get; set; }

		public double? Leaves { get; set; }

		public double? Casual { get; set; }

		public double? Sick { get; set; }

		public double? Annual { get; set; }

		public double? CPL { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }



    }
}