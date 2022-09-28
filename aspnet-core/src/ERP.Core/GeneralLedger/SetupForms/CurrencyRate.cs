using ERP.CommonServices;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms
{
	[Table("CSCURRATE")]
    public class CurrencyRate : Entity<string> , IMustHaveTenant
    {
		public int TenantId { get; set; }

        [Column("CURID")]
        public override string Id { get => base.Id; set => base.Id = value; }
		
	
		public virtual DateTime AUDTDATE { get; set; }
		
	
		public virtual string AUDTUSER { get; set; }
		
		
		public virtual string CURNAME { get; set; }
		
		
		public virtual string SYMBOL { get; set; }
	
		public virtual DateTime RATEDATE { get; set; }
	
		public virtual double CURRATE { get; set; }

        public virtual  int Decimal { get; set; }
        public virtual string Narration { get; set; }
        public virtual string Unit { get; set; }

        //public virtual string CMPID { get; set; }

        //      [ForeignKey("CMPID")]
        //public CompanyProfile CompanyProfileFk { get; set; }

    }
}