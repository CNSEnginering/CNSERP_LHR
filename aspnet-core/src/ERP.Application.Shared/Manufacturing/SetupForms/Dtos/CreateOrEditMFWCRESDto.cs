using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class CreateOrEditMFWCRESDto : EntityDto<int?>
    {

        public int? DetID { get; set; }
        public int? SrNo { get; set; }

        [Required]
        [StringLength(MFWCRESConsts.MaxWCIDLength, MinimumLength = MFWCRESConsts.MinWCIDLength)]
        public string WCID { get; set; }

        [Required]
        [StringLength(MFWCRESConsts.MaxRESIDLength, MinimumLength = MFWCRESConsts.MinRESIDLength)]
        public string RESID { get; set; }

        [Required]
        [StringLength(MFWCRESConsts.MaxRESDESCLength, MinimumLength = MFWCRESConsts.MinRESDESCLength)]
        public string RESDESC { get; set; }

        [StringLength(MFWCRESConsts.MaxUOMLength, MinimumLength = MFWCRESConsts.MinUOMLength)]
        public string UOM { get; set; }

        public double? REQQTY { get; set; }

        public double? UNITCOST { get; set; }

        public double? TOTALCOST { get; set; }

    }
}