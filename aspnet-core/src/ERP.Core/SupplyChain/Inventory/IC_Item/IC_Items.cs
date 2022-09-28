using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.SupplyChain.Inventory.IC_Item
{
    [Table("ICITEM")]
    public class ICItem : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual string ItemId { get; set; }
        public virtual string alternateItemID { get; set; }

        [Required]
        public virtual string Descp { get; set; }
        [Required]
        [Column(TypeName = "char(2)")]
        public virtual string Seg1Id { get; set; }
        [Required]
        [Column(TypeName = "char(6)")]
        public virtual string Seg2Id { get; set; }
        [Required]
        [Column(TypeName = "char(9)")]
        public virtual string Seg3Id { get; set; }

        public virtual DateTime? CreationDate { get; set; }

        public virtual int? ItemCtg { get; set; }

        public virtual int? ItemType { get; set; }

        public virtual int? ItemStatus { get; set; }

        public virtual string StockUnit { get; set; }

        public virtual int? Packing { get; set; }

        public virtual double? Weight { get; set; }

        public virtual bool? Taxable { get; set; }

        public virtual bool? Saleable { get; set; }

        public virtual bool? Active { get; set; }

        public virtual string Barcode { get; set; }

        public virtual string AltItemID { get; set; }

        public virtual string AltDescp { get; set; }

        public virtual string Opt1 { get; set; }

        public virtual string Opt2 { get; set; }

        public virtual string Opt3 { get; set; }

        public virtual int? Opt4 { get; set; }

        public virtual int? Opt5 { get; set; }

        public virtual string DefPriceList { get; set; }

        public virtual string DefVendorAC { get; set; }

        public virtual int? DefVendorID { get; set; }

        public virtual string DefCustAC { get; set; }

        public virtual int? DefCustID { get; set; }

        public virtual string DefTaxAuth { get; set; }

        public virtual int? DefTaxClassID { get; set; }

        public virtual Guid? Picture { get; set; }

        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        public virtual decimal? Conver { get; set; }

        public virtual string ItemSrNo { get; set; }

        public virtual string Venitemcode { get; set; }

        public virtual string VenSrNo { get; set; }

        public virtual string VenLotNo { get; set; }

        public virtual DateTime? ManufectureDate { get; set; }

        public virtual DateTime? Expirydate { get; set; }

        public virtual string warrantyinfo { get; set; }

    }
}
