using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory
{
	[Table("ICSETUP")]
    public class ICSetup : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual string Segment1 { get; set; }
		
		public virtual string Segment2 { get; set; }
        public string Currency { get; set; }
        public virtual int? conType { get; set; }

        public virtual string Segment3 { get; set; }
        public int? InventoryPoint { get; set; }
        
        public virtual bool AllowNegative { get; set; }
		
		public virtual int? ErrSrNo { get; set; }
        public string TransType { get; set; }
		public virtual int? DamageLocID { get; set; }
		
		public virtual int? CostingMethod { get; set; }
		
		public virtual string PRBookID { get; set; }
		
		public virtual string RTBookID { get; set; }
		
		public virtual string CnsBookID { get; set; }
		
		public virtual string SLBookID { get; set; }
		
		public virtual string SRBookID { get; set; }
		
		public virtual string TRBookID { get; set; }
		
		public virtual string PrdBookID { get; set; }
		
		public virtual string PyRecBookID { get; set; }
		
		public virtual string AdjBookID { get; set; }
		
		public virtual string AsmBookID { get; set; }
		
		public virtual string WSBookID { get; set; }
		
		public virtual string DSBookID { get; set; } 
		
		public virtual bool SalesReturnLinkOn { get; set; }
		
		public virtual bool SalesLinkOn { get; set; }
		
		public virtual bool AccLinkOn { get; set; }
		
		public virtual int? CurrentLocID { get; set; }

		public virtual int? GLSegLink { get; set; }
		
		public virtual bool AllowLocID { get; set; }
		
		public virtual bool CDateOnly { get; set; }
		
		public virtual string Opt4 { get; set; }
		
		public virtual string Opt5 { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateadOn { get; set; }
		

    }
}