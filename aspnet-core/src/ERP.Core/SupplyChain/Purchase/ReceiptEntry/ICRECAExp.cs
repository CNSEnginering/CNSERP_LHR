using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Purchase.ReceiptEntry
{
	[Table("ICRECAE")]
    public class ICRECAExp : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int DetID { get; set; }
		
		public virtual int? LocID { get; set; }
		
		public virtual int? DocNo { get; set; }
		
		public virtual string ExpType { get; set; }
		
		[Required]
		public virtual string AccountID { get; set; }
		
		public virtual double? Amount { get; set; }
		

    }
}