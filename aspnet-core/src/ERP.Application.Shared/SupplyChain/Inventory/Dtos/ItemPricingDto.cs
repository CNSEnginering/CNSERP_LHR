
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class ItemPricingDto : EntityDto
    {
        public string PriceList { get; set; }

        public string ItemID { get; set; }

        public string ItemName { get; set; }

        public int priceType { get; set; }

        public decimal? Price { get; set; }

        public double? DiscValue { get; set; }

        public decimal? NetPrice { get; set; }

        public short? Active { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string DocDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SalePrice { get; set; }
        public DateTime ItemDate { get; set; }

    }
}