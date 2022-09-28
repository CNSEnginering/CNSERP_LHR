using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.SalaryLock
{
    [Table("SalaryLock")]
    public class SalaryLock : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        //public virtual int? TenantID { get; set; }

        public virtual int? SalaryMonth { get; set; }

        public virtual int? SalaryYear { get; set; }
        public virtual bool? JVLocked { get; set; }

        public virtual bool Locked { get; set; }

        public virtual DateTime? LockDate { get; set; }

    }
}