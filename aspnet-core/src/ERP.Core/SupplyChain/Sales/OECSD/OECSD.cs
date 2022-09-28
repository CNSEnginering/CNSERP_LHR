using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Sales.OECSD
{
    [Table("OECSD")]
    public class OECSD : Entity, IMustHaveTenant
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

        [StringLength(OECSDConsts.MaxItemIDLength, MinimumLength = OECSDConsts.MinItemIDLength)]
        public virtual string ItemID { get; set; }

        [StringLength(OECSDConsts.MaxUnitLength, MinimumLength = OECSDConsts.MinUnitLength)]
        public virtual string Unit { get; set; }

        public virtual decimal? Conver { get; set; }

        public virtual decimal? Qty { get; set; }

        public virtual decimal? Rate { get; set; }

        public virtual decimal? Amount { get; set; }

        [StringLength(OECSDConsts.MaxTaxAuthLength, MinimumLength = OECSDConsts.MinTaxAuthLength)]
        public virtual string TaxAuth { get; set; }

        public virtual int? TaxClass { get; set; }

        public virtual double? TaxRate { get; set; }

        public virtual double? TaxAmt { get; set; }

        public virtual double? NetAmount { get; set; }

        [StringLength(OECSDConsts.MaxRemarksLength, MinimumLength = OECSDConsts.MinRemarksLength)]
        public virtual string Remarks { get; set; }

    }
}