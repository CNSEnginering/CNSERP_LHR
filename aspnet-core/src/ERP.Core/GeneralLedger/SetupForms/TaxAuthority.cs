using ERP.CommonServices;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms
{
	[Table("CSTAXAUTH")]
    public class TaxAuthority : Entity<string> , IMayHaveTenant
    {
        [Column("TAXAUTH")]
        public override string Id { get => base.Id; set => base.Id = value; }
        public int? TenantId { get; set; }
    	
		[Required]
		public virtual string TAXAUTHDESC { get; set; }
		
		
		public virtual string ACCLIABILITY { get; set; }
		
		
		public virtual string ACCRECOVERABLE { get; set; }
		
		
		public virtual double RECOVERRATE { get; set; }
		
	
		public virtual string ACCEXPENSE { get; set; }
		
		
		public virtual DateTime AUDTDATE { get; set; }
		
		
		public virtual string AUDTUSER { get; set; }
		

	
	
		
    }
}