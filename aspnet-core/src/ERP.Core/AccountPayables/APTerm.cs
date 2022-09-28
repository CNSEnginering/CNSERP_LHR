using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.AccountPayables
{
	[Table("APTERMS")]
    public class APTerm : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }

        [Column("TERMID")]
        public override int Id { get => base.Id; set => base.Id = value; }

        public virtual string TERMDESC { get; set; }
		
		public virtual double? TERMRATE { get; set; }
		
		public virtual DateTime? AUDTDATE { get; set; }
		
		public virtual string AUDTUSER { get; set; }
		
		[Required]
		public virtual bool INACTIVE { get; set; }

        public virtual int TermType { get; set; }

        public virtual int TaxStatus { get; set; }

    }
}