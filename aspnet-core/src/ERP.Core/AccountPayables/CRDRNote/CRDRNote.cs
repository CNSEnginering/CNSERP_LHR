using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;


namespace ERP.AccountPayables.CRDRNote
{
    [Table("GLCRDR")]
    public class CRDRNote : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [Required]
        public virtual int LocID { get; set; }

        [Required]
        public virtual int DocNo { get; set; }

        public virtual DateTime? DocDate { get; set; }

        public virtual DateTime? PostingDate { get; set; }
        public virtual DateTime? PaymentDate { get; set; }

        public virtual short? TypeID { get; set; }

        public virtual string TransType { get; set; }

        public virtual string AccountID { get; set; }

        public virtual int? SubAccID { get; set; }

        public virtual int? InvoiceNo { get; set; }

        public virtual string PartyInvNo { get; set; }
        public virtual DateTime? PartyInvDate { get; set; }
        public virtual double? PartyInvAmount { get; set; }
        public virtual double? Amount { get; set; }

        public virtual string Reason { get; set; }
        public virtual string StkAccID { get; set; }

        public virtual bool Posted { get; set; }

        public virtual int? LinkDetID { get; set; }

        public virtual bool Active { get; set; }

        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }


    }
}
