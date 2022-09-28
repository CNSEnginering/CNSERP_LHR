using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms.LCExpenses
{
    [Table("GLLCEXP")]
    public class LCExpenses : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        public virtual int ExpID { get; set; }
        [Required]
        public virtual string ExpDesc { get; set; }
        public virtual bool Active { get; set; }
        public virtual string AuditUser { get; set; }
        public virtual DateTime? AuditDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreateDate { get; set; }
    }
}
