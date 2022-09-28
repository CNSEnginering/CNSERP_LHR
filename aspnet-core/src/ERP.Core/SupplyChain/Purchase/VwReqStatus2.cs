using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Purchase
{
	[Table("vwReqStatus2")]
    public class VwReqStatus2 : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual int? Locid { get; set; }
		
		public virtual int? Docno { get; set; }
		
		public virtual string ItemID { get; set; }
		
		[Required]
		public virtual string Descp { get; set; }
		
		public virtual string Unit { get; set; }
		
		public virtual double? Conver { get; set; }
		
		public virtual double? ReqQty { get; set; }
		
		public virtual double? QIH { get; set; }
		
		public virtual double? POQty { get; set; }
		
		public virtual double? Received { get; set; }
		
		public virtual double? Returned { get; set; }
		
		public virtual double? QtyP { get; set; }
		

    }
}