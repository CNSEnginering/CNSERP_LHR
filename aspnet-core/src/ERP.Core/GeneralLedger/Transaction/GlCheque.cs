using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.Transaction
{
    [Table("glpdc")]
    public class GlCheque : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual int? DocID { get; set; }

        public virtual int? TypeID { get; set; }

        public virtual DateTime? EntryDate { get; set; }

        public virtual DateTime? ChequeDate { get; set; }
        public virtual DateTime? StatusDate { get; set; }

        public virtual string ChequeNo { get; set; }

        public virtual double? ChequeAmt { get; set; }

        public virtual int? ChequeStatus { get; set; }

        public virtual string PartyBank { get; set; }

        public virtual string ChequeRef { get; set; }

        public virtual string Remarks { get; set; }

        public virtual int? LocationID { get; set; }

        public virtual string AccountID { get; set; }

        public virtual int? PartyID { get; set; }

        public virtual string BankID { get; set; }

        public virtual bool Posted { get; set; }

        public virtual string AUDTUSER { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreatedDate { get; set; }
        public virtual byte? ChType { get; set; }
        public virtual string BankAccountID { get; set; }
        public virtual int? ConfigID { get; set; }
        public int? LinkDetID { get; set; }
    }
}