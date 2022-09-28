using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms.LCExpensesHD
{
    [Table("GLLCD")]
    public class LCExpensesDetail : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [Required]
        public virtual int DetID { get; set; }
        [Required]
        public virtual int LocID { get; set; }
        [Required]
        public virtual int DocNo { get; set; }
        [Required]
        public virtual string ExpDesc { get; set; }
        public virtual double? Amount { get; set; }
    }

}
