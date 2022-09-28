
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.EmployeeSalary.Dtos
{
    public class CreateOrEditEmployeeSalaryDto : EntityDto<int?>
    {

        [Required]
        public int EmployeeID { get; set; }


        [Required]
        public string EmployeeName { get; set; }

        [Required]
        public double Bank_Amount { get; set; }



        [Required]
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