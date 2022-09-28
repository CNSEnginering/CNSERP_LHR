using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.EmployeeSalary
{
    [Table("EmployeeSalary")]
    public class EmployeeSalary : Entity, IMustHaveTenant
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