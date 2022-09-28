using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory.AssetRegistration
{
	[Table("AMRegisterDetail")]
    public class AssetRegistrationDetail : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual int? AssetID { get; set; }
		
		public virtual DateTime? DepStartDate { get; set; }
		
		public virtual short? DepMethod { get; set; }
		
		public virtual decimal? AssetLife { get; set; }
		
		public virtual decimal? BookValue { get; set; }
		
		public virtual decimal? LastDepAmount { get; set; }
		
		public virtual DateTime? LastDepDate { get; set; }
		
		public virtual decimal? AccumulatedDepAmt { get; set; }
		

    }
}