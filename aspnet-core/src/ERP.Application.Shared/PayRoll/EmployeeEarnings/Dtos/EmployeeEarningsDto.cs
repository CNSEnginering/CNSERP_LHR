
using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.EmployeeEarnings.Dtos
{
    public class EmployeeEarningsDto : EntityDto
    {
        public int EarningID { get; set; }

        public int EmployeeID { get; set; }
        public int? EarningTypeID { get; set; }

        public string EmployeeName { get; set; }
        public string Remarks { get; set; }

        public short SalaryYear { get; set; }

        public short SalaryMonth { get; set; }

        public DateTime? EarningDate { get; set; }

        public double? Amount { get; set; }

        public bool Active { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }



    }
}