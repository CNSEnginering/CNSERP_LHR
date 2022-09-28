using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Payroll.SlabSetup
{
    [Table("SlabSetup")]
    public class SlabSetup : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public virtual int? TypeID { get; set; }

        public virtual double? SlabFrom { get; set; }

        public virtual double? SlabTo { get; set; }

        public virtual double? Rate { get; set; }

        public virtual double? Amount { get; set; }

        public virtual bool Active { get; set; }

        [StringLength(SlabSetupConsts.MaxAudtUserLength, MinimumLength = SlabSetupConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        [StringLength(SlabSetupConsts.MaxCreatedByLength, MinimumLength = SlabSetupConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

        [StringLength(SlabSetupConsts.MaxModifiedByLength, MinimumLength = SlabSetupConsts.MinModifiedByLength)]
        public virtual string ModifiedBy { get; set; }

        public virtual DateTime? ModifyDate { get; set; }

    }
}