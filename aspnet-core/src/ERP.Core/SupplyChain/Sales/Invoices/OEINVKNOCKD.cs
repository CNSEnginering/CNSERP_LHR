using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Sales.Invoices
{
    [Table("OEINVKNOCKD")]
    public class OEINVKNOCKD : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public virtual int? DetID { get; set; }

        public virtual int? DocNo { get; set; }

        [Required]
        public virtual int InvNo { get; set; }

        [StringLength(OEINVKNOCKDConsts.MaxInvDateLength, MinimumLength = OEINVKNOCKDConsts.MinInvDateLength)]
        public virtual string InvDate { get; set; }

        public virtual double? Amount { get; set; }

        public virtual double? AlreadyPaid { get; set; }

        public virtual double? Pending { get; set; }

        public virtual double? Adjust { get; set; }

        [StringLength(OEINVKNOCKDConsts.MaxCreatedByLength, MinimumLength = OEINVKNOCKDConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreatedDate { get; set; }

        [StringLength(OEINVKNOCKDConsts.MaxAudtUserLength, MinimumLength = OEINVKNOCKDConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

    }
}