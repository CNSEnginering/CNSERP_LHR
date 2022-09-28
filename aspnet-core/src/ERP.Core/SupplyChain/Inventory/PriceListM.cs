using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory
{
	[Table("ICPRICM")]
    public class PriceListsM : Entity , IMayHaveTenant
    {
		public int? TenantId { get; set; }
			
		[Required]
		public virtual string PriceList { get; set; }
        public virtual DateTime DocDate { get; set; }

        public virtual short Active { get; set; }
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }
    }
}