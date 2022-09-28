using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.EmployeeLeaves
{
    [Table("EmployeeLeaves")]
    public class EmployeeLeaves : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }


        [Required]
        public virtual int EmployeeID { get; set; }

        //[Required]
        //public virtual string EmployeeName { get; set; }

        [Required]
        public virtual int LeaveID { get; set;}

        [Required]
        public virtual int SalaryYear { get; set; }

        public virtual short? SalaryMonth { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual double? LeaveType { get; set; }

        public virtual double? Casual { get; set; }

        public virtual double? Sick { get; set; }

        public virtual double? Annual { get; set; }

        public virtual string PayType { get; set; }

        public virtual string Remarks { get; set; }

        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }



    }
}