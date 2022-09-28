
using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.EmployeeSalary.Dtos
{
    public class EmployeeSalaryDto : EntityDto
    {
        public int EmployeeID { get; set; }

        public string EmployeeName { get; set; }
        public double Bank_Amount { get; set; }

        public DateTime StartDate { get; set; }

        public double? Gross_Salary { get; set; }

        public double? Basic_Salary { get; set; }

        public double? Tax { get; set; }

        public double? House_Rent { get; set; }

        public double? Net_Salary { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }



    }
}