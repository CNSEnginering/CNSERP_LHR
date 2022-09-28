using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms
{
    [Table("vwGlSec")]
    public class GLSecChartofControl : Entity<string> , IMustHaveTenant
    {
			public int TenantId { get; set; }

        [Column("AccountID")]
        public override string Id { get => base.Id; set => base.Id = value; }
		
		[Required]
		public virtual string AccountName { get; set; }
		
		[Required]
		public virtual bool SubLedger { get; set; }

        [Column("Segment1")]
        public virtual string ControlDetailId { get; set; }


        [Column("Segment2")]
        public virtual string SubControlDetailId { get; set; }


        [Column("Segment3")]
        public virtual string Segmentlevel3Id { get; set; }

        public virtual int? OptFld { get; set; }
		
		public virtual short? SLType { get; set; }
		
		public virtual bool Inactive { get; set; }
		
		public virtual DateTime? CreationDate { get; set; }
		
		public virtual string AuditUser { get; set; }
		
		public virtual DateTime? AuditTime { get; set; }
		
		public virtual string OldCode { get; set; }	

        public int GroupCode { get; set; }
    }
}