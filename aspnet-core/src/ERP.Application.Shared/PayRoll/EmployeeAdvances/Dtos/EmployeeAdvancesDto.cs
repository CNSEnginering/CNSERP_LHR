
using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.EmployeeAdvances.Dtos
{
    public class EmployeeAdvancesDto : EntityDto
    {
		public int AdvanceID { get; set; }

		public int EmployeeID { get; set; }

		public string EmployeeName { get; set; }
		public string Remarks { get; set; }

		public short SalaryYear { get; set; }

		public short SalaryMonth { get; set; }

		public DateTime? AdvanceDate { get; set; }

		public double? Amount { get; set; }

		public bool Active { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }



    }
}