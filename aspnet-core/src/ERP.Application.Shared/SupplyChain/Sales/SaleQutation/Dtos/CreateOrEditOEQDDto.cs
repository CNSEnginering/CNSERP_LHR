using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Sales.SaleQutation.Dtos
{
    public class CreateOrEditOEQDDto : EntityDto<int?>
    {

        [Required]
        public int DetID { get; set; }

        [Required]
        public int LocID { get; set; }

        [Required]
        public int DocNo { get; set; }

        [Required]
        public int TransType { get; set; }

        [StringLength(OEQDConsts.MaxItemIDLength, MinimumLength = OEQDConsts.MinItemIDLength)]
        public string ItemID { get; set; }

        [StringLength(OEQDConsts.MaxUnitLength, MinimumLength = OEQDConsts.MinUnitLength)]
        public string Unit { get; set; }

        public decimal? Conver { get; set; }

        public decimal? Qty { get; set; }

        public decimal? Rate { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(OEQDConsts.MaxTaxAuthLength, MinimumLength = OEQDConsts.MinTaxAuthLength)]
        public string TaxAuth { get; set; }

        public int? TaxClass { get; set; }

        public double? TaxRate { get; set; }

        public double? TaxAmt { get; set; }

        public double? NetAmount { get; set; }

        [StringLength(OEQDConsts.MaxRemarksLength, MinimumLength = OEQDConsts.MinRemarksLength)]
        public string Remarks { get; set; }

    }
}