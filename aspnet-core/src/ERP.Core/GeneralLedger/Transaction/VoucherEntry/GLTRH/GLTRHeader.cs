using ERP.GeneralLedger.SetupForms;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH
{
    [Table("GLTRH")]
    public class GLTRHeader : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Column("DetID")]
        public override int Id { get => base.Id; set => base.Id = value; }

        [Required]
        public virtual string BookID { get; set; }

        [Required]
        public virtual int ConfigID { get; set; }

        [Required]
        public virtual int DocNo { get; set; }

        public virtual int FmtDocNo { get; set; }

        [Required]
        public virtual int DocMonth { get; set; }

        [Required]
        public virtual DateTime DocDate { get; set; }

        public virtual string NARRATION { get; set; }

        [Required]
        public virtual bool Posted { get; set; }
        public virtual bool Reversed { get; set; }
        public virtual string PostedBy { get; set; }

        public virtual DateTime? PostedDate { get; set; }

        public virtual bool Approved { get; set; }

        public virtual string AprovedBy { get; set; }

        public virtual DateTime? AprovedDate { get; set; }

        public virtual string AuditUser { get; set; }

        public virtual DateTime? AuditTime { get; set; }

        public virtual string OldCode { get; set; }

        public virtual string CURID { get; set; }

        public virtual double? CURRATE { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreatedOn { get; set; }
        public virtual byte? ChType { get; set; }
        public virtual string ChNumber { get; set; }
        public virtual decimal? Amount { get; set; }

        public virtual int LocId { get; set; }
        public string Reference { get; set; }


        //public virtual string GLCONFIGId { get; set; }


        // [ForeignKey("GLCONFIGId")]
        //public GLCONFIG GLCONFIGFk { get; set; }

    }
}