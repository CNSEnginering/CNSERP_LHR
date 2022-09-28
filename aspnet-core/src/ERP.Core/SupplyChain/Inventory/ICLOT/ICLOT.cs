using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory.ICLOT
{
    [Table("ICLOT")]
    public class ICLOT : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        //public virtual int? TenantID { get; set; }

        [StringLength(ICLOTConsts.MaxLotNoLength, MinimumLength = ICLOTConsts.MinLotNoLength)]
        public virtual string LotNo { get; set; }
        
        public virtual DateTime? ManfDate { get; set; }
       
        public virtual DateTime? ExpiryDate { get; set; }

        public virtual bool Active { get; set; }

        [StringLength(ICLOTConsts.MaxAudtUserLength, MinimumLength = ICLOTConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        [StringLength(ICLOTConsts.MaxCreatedByLength, MinimumLength = ICLOTConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

    }
}