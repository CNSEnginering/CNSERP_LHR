using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms
{
	[Table("GLBOOKS")]
    public class GLBOOKS : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual string BookID { get; set; }
		
		[Required]
		public virtual string BookName { get; set; }
		
		[Required]
		public virtual int NormalEntry { get; set; }
		
		public virtual bool Integrated { get; set; }
		
		public virtual bool INACTIVE { get; set; }
		
		public virtual DateTime? AUDTDATE { get; set; }
		
		public virtual string AUDTUSER { get; set; }
		
		public virtual short? Restricted { get; set; }
		

    }
}