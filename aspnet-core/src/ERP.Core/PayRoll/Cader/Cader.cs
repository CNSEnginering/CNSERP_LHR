using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.Cader
{
    [Table("Cader")]
    public class Cader : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [StringLength(CaderConsts.MaxCADER_NAMELength, MinimumLength = CaderConsts.MinCADER_NAMELength)]
        public virtual string CADER_NAME { get; set; }

        [StringLength(CaderConsts.MaxAudtUserLength, MinimumLength = CaderConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        [StringLength(CaderConsts.MaxCreatedByLength, MinimumLength = CaderConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

    }
}