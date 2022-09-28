using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD
{
	[Table("GLTRD")]
    public class GLTRDetail : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }

        [Column("DetailID")]
        public override int Id { get => base.Id; set => base.Id = value; }

        [Required]
		public virtual int DetID { get; set; }
		
		[Required]
		public virtual string AccountID { get; set; }
		
		public virtual int? SubAccID { get; set; }
		
		public virtual string Narration { get; set; }
		
		public virtual double? Amount { get; set; }
		
		public virtual string ChequeNo { get; set; }

        public virtual bool IsAuto { get; set; }

        public virtual int LocId { get; set; }

    }
}