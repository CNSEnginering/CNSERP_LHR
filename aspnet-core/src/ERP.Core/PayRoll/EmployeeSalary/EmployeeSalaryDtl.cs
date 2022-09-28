using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.PayRoll.EmployeeSalary
{
    [Table("EmployeeSalaryDtl")]
    public class EmployeeSalaryDtl : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }


        [Required]
        public virtual int EmployeeID { get; set; }

        [Required]
        public virtual string EmployeeName { get; set; }

        [Required]
        public virtual double Bank_Amount { get; set; }

        [Required]
        public virtual DateTime StartDate { get; set; }

        public virtual double? Gross_Salary { get; set; }

        public virtual double? Basic_Salary { get; set; }

        public virtual double? Tax { get; set; }

        public virtual double? House_Rent { get; set; }

        public virtual double? Net_Salary { get; set; }

        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }


    }
}

