using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Purchase
{
	[Table("vwReqStatus")]
    public class VwReqStatus : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int LocID { get; set; }
		
		[Required]
		public virtual int DocNo { get; set; }
		
		public virtual DateTime? DocDate { get; set; }
		
		public virtual string Narration { get; set; }
		
		public virtual string ItemID { get; set; }
		
		[Required]
		public virtual string Descp { get; set; }
		
		public virtual double? reqqty { get; set; }
		
		[Required]
		public virtual double poqty { get; set; }
		
		[Required]
		public virtual double recqty { get; set; }
		
		public virtual DateTime? podate { get; set; }
		
		public virtual DateTime? recdate { get; set; }
		
		public virtual string party_name { get; set; }
		
		public virtual string location { get; set; }
		
		public virtual string rec_narration { get; set; }
		
		public virtual string OrdNo { get; set; }
		
		public virtual int? recdocno { get; set; }
		
		public virtual int? podocno { get; set; }
		
		public virtual int? SUBCCID { get; set; }
        public string SubAccName { get; set; }


    }
}