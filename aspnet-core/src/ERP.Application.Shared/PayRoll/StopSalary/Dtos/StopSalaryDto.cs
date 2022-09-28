
using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.StopSalary.Dtos
{
    public class StopSalaryDto : EntityDto
    {
        public int? stopSalaryID { get; set; }
        public int? TypeID { get; set; }
        public int? LoanID { get; set; }

        public int EmployeeID { get; set; }

		public short SalaryYear { get; set; }

		public short SalaryMonth { get; set; }

		public string Remarks { get; set; }

		public bool Active { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }

        public double? Amount{ get; set; }

        public string EmployeeName { get; set; }
        public bool? Include { get; set; }

    }
}