using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory
{
	[Table("vwPriceList")]
    public class ItemsPriceList : Entity , IMustHaveTenant
    {
		public int TenantId { get; set; }
			
		public virtual string PriceList { get; set; }

		public virtual string ItemID { get; set; }

		public virtual string Descp { get; set; }
		public virtual string StockUnit { get; set; }
		public virtual decimal PurPrice { get; set; }
		public virtual decimal SalesPrice { get; set; }
		
		public virtual double DiscValue { get; set; }
		
		public virtual decimal NetPrice { get; set; }

    }
}