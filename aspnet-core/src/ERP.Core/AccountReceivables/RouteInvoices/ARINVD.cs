using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.AccountReceivables.RouteInvoices
{
	[Table("ARINVD")]
    public class ARINVD : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int DetID { get; set; }
		
		[Required]
		public virtual string AccountID { get; set; }
		
		public virtual int? SubAccID { get; set; }
		
		public virtual int? DocNo { get; set; }
		
		public virtual string InvNumber { get; set; }
		
		public virtual double? InvAmount { get; set; }
		
		public virtual double? TaxAmount { get; set; }
		
		public virtual double? RecpAmount { get; set; }
		
		public virtual string ChequeNo { get; set; }
		
		public virtual bool Adjust { get; set; }
		
		public virtual string Narration { get; set; }
		

    }
}