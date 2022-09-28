using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Purchase
{
    [Table("POSTAT")]
    public class POSTAT : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }


        [Required]
        public virtual int DetID { get; set; }

        [Required]
        public virtual int LocID { get; set; }

        [Required]
        public virtual int DocNo { get; set; }
        public virtual string SubAccName { get; set; }

        public virtual string ItemID { get; set; }

        public virtual string Unit { get; set; }

        public virtual double? Conver { get; set; }

        public virtual double? Qty { get; set; }

        public virtual double? Rate { get; set; }

        public virtual double? Amount { get; set; }

        public virtual string Remarks { get; set; }

        [Required]
        public virtual double Received { get; set; }

        [Required]
        public virtual double Returned { get; set; }

        public virtual long? ReqNo { get; set; }

        public virtual DateTime? DocDate { get; set; }
        public virtual DateTime? ArrivalDate { get; set; }

        public virtual double? QtyP { get; set; }

        public virtual double? QtyPAmt { get; set; }
        public virtual string LedgerDesc { get; set; }
        public virtual string TaxAuth { get; set; }
        public virtual int? TaxClass { get; set; }
        public virtual double? TaxRate { get; set; }
        public virtual double? TaxAmt { get; set; }
        public virtual string CLASSDESC { get; set; }
    }
}