using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.Transaction.BankReconcile
{
    [Table("GLReconH")]
    public class BankReconcile : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public virtual string DocID { get; set; }

        public virtual DateTime? DocDate { get; set; }

        public virtual string BankID { get; set; }

        public virtual string BankName { get; set; }

        public virtual double? BeginBalance { get; set; }

        public virtual double? EndBalance { get; set; }

        public virtual double? ReconcileAmt { get; set; }

        public virtual double? DiffAmount { get; set; }
        public virtual double? StatementAmt { get; set; }

        public virtual double? ClDepAmt { get; set; }

        public virtual double? ClPayAmt { get; set; }

        public virtual double? UnClDepAmt { get; set; }

        public virtual double? UnClPayAmt { get; set; }

        public virtual int? ClItems { get; set; }

        public virtual int? UnClItems { get; set; }

        public virtual string Narration { get; set; }

        public virtual bool? Completed { get; set; }

        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreatedDate { get; set; }
         
        public virtual int DocNo { get; set; }

        public virtual bool? Approved { get; set; }
        public virtual string ApprovedBy { get; set; }
        public virtual DateTime? ApprovedDatetime { get; set; }
    }
}