using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.Adjustments
{
    [Table("AdjH")]
    public class AdjH : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public virtual int? DocType { get; set; }

        public virtual int? TypeID { get; set; }

        public virtual int? DocID { get; set; }

        public virtual DateTime? Docdate { get; set; }

        [Required]
        public virtual short SalaryYear { get; set; }

        [Required]
        public virtual short SalaryMonth { get; set; }

        [StringLength(AdjHConsts.MaxAudtUserLength, MinimumLength = AdjHConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        [StringLength(AdjHConsts.MaxCreatedByLength, MinimumLength = AdjHConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }


    }
}