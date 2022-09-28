using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Sales.SaleEntry
{
	[Table("OESALED")]
    public class OESALEDetail : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int DetID { get; set; }
		
		[Required]
		public virtual int LocID { get; set; }
		
		[Required]
		public virtual int DocNo { get; set; }
		
		[Required]
		public virtual string ItemID { get; set; }
		
		public virtual string Unit { get; set; }
		
		public virtual double? Conver { get; set; }
		
		public virtual double? Qty { get; set; }
		
		public virtual double? Rate { get; set; }
		
		public virtual double? Amount { get; set; }

        public virtual double? ExlTaxAmount { get; set; }
        

        public virtual double? Disc { get; set; }
		
		public virtual string TaxAuth { get; set; }
		
		public virtual int? TaxClass { get; set; }
		
		public virtual double? TaxRate { get; set; }
		
		public virtual double? TaxAmt { get; set; }
		
		public virtual string Remarks { get; set; }
		
		public virtual double? NetAmount { get; set; }
		

    }
}