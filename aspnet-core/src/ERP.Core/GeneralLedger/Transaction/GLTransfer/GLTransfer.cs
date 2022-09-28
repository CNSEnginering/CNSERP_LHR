using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;


namespace ERP.GeneralLedger.Transaction.GLTransfer
{
    [Table("GLTRANSFER")]
    public class GLTransfer : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [Required]
        public virtual int DOCID { get; set; }

        [Required]
        public virtual DateTime DOCDATE { get; set; }

        [Required]
        public virtual DateTime TRANSFERDATE { get; set; }

        public virtual string DESCRIPTION { get; set; }
        public virtual int? FROMLOCID { get; set; }
        public virtual string FROMBANKID { get; set; }
        public virtual int? FROMCONFIGID { get; set; }
        public virtual string FROMBANKACCID { get; set; }
        public virtual string FROMACCID { get; set; }
        public virtual int? TOLOCID { get; set; }
        public virtual string TOBANKID { get; set; }
        public virtual int? TOCONFIGID { get; set; }
        public virtual string TOBANKACCID { get; set; }
        public virtual string TOACCID { get; set; }
        public virtual double? TRANSFERAMOUNT { get; set; }
        public virtual bool STATUS { get; set; }
        public virtual int? GLLINKIDFROM { get; set; }
        public virtual int? GLLINKIDTO { get; set; }
        public virtual int? GLDOCIDFROM { get; set; }
        public virtual int? GLDOCIDTO { get; set; }
        public virtual byte? ChType { get; set; }
        public virtual string ChNumber { get; set; }

        public virtual string AUDTUSER { get; set; }

        public virtual DateTime? AUDTDATE { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreatedOn { get; set; }
        public virtual int? LinkDetIDBP { get; set; }
        public virtual int? LinkDetIDBR { get; set; }
        public virtual int? LinkDetIDCP { get; set; }
        public virtual int? LinkDetIDCR { get; set; }
        public bool? Posted { get; set; }
    }
}
