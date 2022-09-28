using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory
{
	[Table("ICLEDG")]
    public class ICLEDG : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }

			

		public virtual DateTime? DocDate { get; set; }
		
		[Required]
		public virtual int DocType { get; set; }
		public virtual long SortId { get; set; }
		
		[Required]
		public virtual string DocDesc { get; set; }
		
		[Required]
		public virtual int DocNo { get; set; }
		
		public virtual int? LocID { get; set; }
		
		public virtual string ItemID { get; set; }
		
		public virtual string srno { get; set; }
		
		public virtual string UNIT { get; set; }
		
		public virtual double? Conver { get; set; }
		
		public virtual double? Qty { get; set; }
		
		public virtual double? Rate { get; set; }
		
		public virtual double? Amount { get; set; }
		
		public virtual string Descp { get; set; }
		
		[Required]
		public virtual string TableName { get; set; }
		
		public virtual int? PKID { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		
		public virtual string JobNo { get; set; }
		

    }
}