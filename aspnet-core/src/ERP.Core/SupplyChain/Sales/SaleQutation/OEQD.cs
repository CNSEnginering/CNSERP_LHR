using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Sales.SaleQutation
{
    [Table("OEQD")]
    public class OEQD : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        public virtual int DetID { get; set; }

        [Required]
        public virtual int LocID { get; set; }

        [Required]
        public virtual int DocNo { get; set; }

        [Required]
        public virtual int TransType { get; set; }

        [StringLength(OEQDConsts.MaxItemIDLength, MinimumLength = OEQDConsts.MinItemIDLength)]
        public virtual string ItemID { get; set; }

        [StringLength(OEQDConsts.MaxUnitLength, MinimumLength = OEQDConsts.MinUnitLength)]
        public virtual string Unit { get; set; }

        public virtual decimal? Conver { get; set; }

        public virtual decimal? Qty { get; set; }

        public virtual decimal? Rate { get; set; }

        public virtual decimal? Amount { get; set; }

        public virtual decimal? ExlTaxAmount { get; set; }



        [StringLength(OEQDConsts.MaxTaxAuthLength, MinimumLength = OEQDConsts.MinTaxAuthLength)]
        public virtual string TaxAuth { get; set; }

        public virtual int? TaxClass { get; set; }

        public virtual double? TaxRate { get; set; }

        public virtual double? TaxAmt { get; set; }

        public virtual double? NetAmount { get; set; }

        [StringLength(OEQDConsts.MaxRemarksLength, MinimumLength = OEQDConsts.MinRemarksLength)]
        public virtual string Remarks { get; set; }

    }
}