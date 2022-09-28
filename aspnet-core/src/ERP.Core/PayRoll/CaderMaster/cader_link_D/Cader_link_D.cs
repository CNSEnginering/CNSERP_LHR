using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.CaderMaster.cader_link_D
{
    [Table("cader_link_D")]
    public class Cader_link_D : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        public virtual int CaderID { get; set; }
        public virtual int DetId { get; set; }
        [Required]
        [StringLength(Cader_link_DConsts.MaxAccountIDLength, MinimumLength = Cader_link_DConsts.MinAccountIDLength)]
        public virtual string AccountID { get; set; }

        [StringLength(Cader_link_DConsts.MaxAccountNameLength, MinimumLength = Cader_link_DConsts.MinAccountNameLength)]
        public virtual string AccountName { get; set; }

        [Required]
        [StringLength(Cader_link_DConsts.MaxTypeLength, MinimumLength = Cader_link_DConsts.MinTypeLength)]
        public virtual string Type { get; set; }

        [Required]
        [StringLength(Cader_link_DConsts.MaxPayTypeLength, MinimumLength = Cader_link_DConsts.MinPayTypeLength)]
        public virtual string PayType { get; set; }

        [StringLength(Cader_link_DConsts.MaxNarrationLength, MinimumLength = Cader_link_DConsts.MinNarrationLength)]
        public virtual string Narration { get; set; }

    }
}