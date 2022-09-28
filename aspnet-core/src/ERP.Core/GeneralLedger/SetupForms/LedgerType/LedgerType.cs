using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms.LedgerType
{
	[Table("GLLedType")]
    public class LedgerType : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int LedgerID { get; set; }
		
		[Required]
		public virtual string LedgerDesc { get; set; }
		
		public virtual bool Active { get; set; }
		

    }
}