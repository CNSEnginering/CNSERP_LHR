using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.Transaction
{
	[Table("GLTRV")]
    public class AccountsPosting : Entity, IMustHaveTenant
    {
        [Column("DetID")]
        public override int Id { get => base.Id; set => base.Id = value; }
		
		[Required]
		public virtual string BookID { get; set; }
		
		[Required]
		public virtual int ConfigID { get; set; }
		
		[Required]
		public virtual int DocNo { get; set; }

        public virtual int FmtDocNo { get; set; }

        public virtual string ChNumber { get; set; }

        [Required]
		public virtual int DocMonth { get; set; }

		public virtual int LocId { get; set; }
		
		[Required]
		public virtual DateTime DocDate { get; set; }
		
		public virtual string AuditUser { get; set; }
		
		public virtual DateTime? AuditTime { get; set; }
		
		[Required]
		public virtual bool Posted { get; set; }
		public virtual bool Approved { get; set; }
		
		public virtual string BookName { get; set; }
		
		public virtual string AccountID { get; set; }
		
		public virtual int? SubAccID { get; set; }
		
		public virtual string Narration { get; set; }
		
		public virtual double? Amount { get; set; }
		
		public virtual string AccountName { get; set; }
		
		public virtual string SubAccName { get; set; }
		
		public virtual int? DetailID { get; set; }
		
		[Required]
		public virtual string ChequeNo { get; set; }
		
		public virtual string RegNo { get; set; }
		
		public virtual string Reference { get; set; }
        public int TenantId { get; set; }

		public virtual string CreatedBy { get; set; }

		public virtual DateTime? CreatedOn { get; set; }
	}
}