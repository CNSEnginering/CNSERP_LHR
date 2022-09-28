using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.Allowances
{
    [Table("AllowancesH")]
    public class Allowances : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }


        public virtual int? DocID { get; set; }

        [Required]
        public virtual DateTime Docdate { get; set; }

        public virtual short? DocMonth { get; set; }
        
        public virtual int? DocYear { get; set; }

        [StringLength(AllowancesConsts.MaxAudtUserLength, MinimumLength = AllowancesConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        [StringLength(AllowancesConsts.MaxCreatedByLength, MinimumLength = AllowancesConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }


    }
}