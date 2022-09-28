using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory
{
    [Table("ICPRIC")]
    public class ItemPricing : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }


        [Required]
        public virtual string PriceList { get; set; }

        [Required]
        public virtual string ItemID { get; set; }

        public virtual int priceType { get; set; }

        public virtual decimal? Price { get; set; }

        public virtual double? DiscValue { get; set; }

        public virtual decimal? NetPrice { get; set; }

        public virtual short? Active { get; set; }

        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }
        public virtual decimal? PurchasePrice { get; set; }
        public virtual decimal? SalePrice { get; set; }
        public virtual DateTime ItemDate { get; set; }


    }
}