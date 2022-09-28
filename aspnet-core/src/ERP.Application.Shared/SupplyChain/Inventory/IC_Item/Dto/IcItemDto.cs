using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.SupplyChain.Inventory.IC_Item.Dto
{
    public class IcItemDto: EntityDto
    {
        [Required]
        public string Seg1Id { get; set; }
        [Required]
        public string Seg2Id { get; set; }
        [Required]
        public string Seg3Id { get; set; }
        public string ItemId { get; set; }
        public string alternateItemID { get; set; }
        public string Descp { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ItemCtg { get; set; }
        public int? ItemType { get; set; }

        public int? ItemStatus { get; set; }

        public string StockUnit { get; set; }

        public int? Packing { get; set; }

        public double? Weight { get; set; }

        public bool? Taxable { get; set; }

        public bool? Saleable { get; set; }

        public bool? Active { get; set; }

        public string Barcode { get; set; }

        public string AltItemID { get; set; }

        public string AltDescp { get; set; }

        public string Opt1 { get; set; }

        public string Opt2 { get; set; }

        public string Opt3 { get; set; }

        public int? Opt4 { get; set; }

        public int? Opt5 { get; set; }

        public string DefPriceList { get; set; }

        public string DefVendorAC { get; set; }

        public int? DefVendorID { get; set; }

        public string DefCustAC { get; set; }

        public int? DefCustID { get; set; }

        public string DefTaxAuth { get; set; }

        public int? DefTaxClassID { get; set; }

        public Guid? Picture { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public decimal? Conver { get; set; }

        public string ItemSrNo { get; set; }

        public string Venitemcode { get; set; }

        public string VenSrNo { get; set; }

        public string VenLotNo { get; set; }

        public DateTime? ManufectureDate { get; set; }

        public DateTime? Expirydate { get; set; }

        public string warrantyinfo { get; set; }
    }
}
