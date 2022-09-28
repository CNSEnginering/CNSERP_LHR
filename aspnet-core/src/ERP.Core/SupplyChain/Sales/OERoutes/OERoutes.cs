using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Sales.OERoutes
{
    [Table("OERoutes")]
    public class OERoutes : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        public virtual int RoutID { get; set; }

        [StringLength(OERoutesConsts.MaxRoutDescLength, MinimumLength = OERoutesConsts.MinRoutDescLength)]
        public virtual string RoutDesc { get; set; }

        public virtual bool Active { get; set; }

        [StringLength(OERoutesConsts.MaxCreatedByLength, MinimumLength = OERoutesConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

        [StringLength(OERoutesConsts.MaxAudtUserLength, MinimumLength = OERoutesConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

    }
}