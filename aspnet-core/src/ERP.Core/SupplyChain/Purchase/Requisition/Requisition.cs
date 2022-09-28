using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Purchase.Requisition
{
    [Table("POREQH")]
    public class Requisitions : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        public virtual int LocID { get; set; }

        [Required]
        public virtual int DocNo { get; set; }

        public virtual DateTime? DocDate { get; set; }

        public virtual DateTime? ExpArrivalDate { get; set; }

        public virtual string OrdNo { get; set; }
        [StringLength(10)]
        public virtual string CCID { get; set; }
        [StringLength(100)]
        public virtual string Narration { get; set; }
        public string BasicStyle { get; set; }
        public string License { get; set; }
        public string PartyName { get; set; }
        public string ItemName { get; set; }
        public double? OrderQty { get; set; }


        public virtual double? TotalQty { get; set; }

        //public virtual DateTime? ArrivalDate { get; set; }
        [StringLength(20)]
        public virtual string ReqNo { get; set; }
        [StringLength(20)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AuditTime { get; set; }

        public virtual decimal? SysDate { get; set; }

        public virtual int? DbID { get; set; }

        public virtual bool Completed { get; set; }

        public virtual bool Active { get; set; }

        public virtual bool Hold { get; set; }

        public virtual DateTime? AudtDate { get; set; }
        [StringLength(20)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

        public virtual bool Approved { get; set; }
        public virtual string ApprovedBy { get; set; }
        public virtual DateTime? ApprovedDate { get; set; }

        public virtual bool Posted { get; set; }
    }
}