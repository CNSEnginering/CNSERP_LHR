using ERP.GeneralLedger.SetupForms;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.CommonServices
{
	[Table("CSTAXCLASS")]
    public class TaxClass : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }

       // [Column("CLASSID")]
       // public override int Id { get => base.Id; set => base.Id = value; }

        public virtual int CLASSID { get; set; }

        public virtual string TAXAUTH { get; set; }
		
		public virtual string CLASSDESC { get; set; }
		
		public virtual double? CLASSRATE { get; set; }
		
		public virtual double? TRANSTYPE { get; set; }
		
		public virtual string CLASSTYPE { get; set; }

        public virtual string TAXACCID { get; set; }

        public virtual bool IsActive { get; set; }
		
		public virtual DateTime? AUDTDATE { get; set; }
		
		public virtual string AUDTUSER { get; set; }
		

		//public virtual string? TaxAuthorityId { get; set; }
		
      //  [ForeignKey("TAXAUTH")]
		//public TaxAuthority TaxAuthorityFk { get; set; }
		
    }
}