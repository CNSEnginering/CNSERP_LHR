using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class CreateOrEditMFWCTOLDto : EntityDto<int?>
    {
        public int? SrNo { get; set; }
        public int? DetID { get; set; }

        [Required]
        [StringLength(MFWCTOLConsts.MaxWCIDLength, MinimumLength = MFWCTOLConsts.MinWCIDLength)]
        public string WCID { get; set; }

        [Required]
        [StringLength(MFWCTOLConsts.MaxTOOLTYIDLength, MinimumLength = MFWCTOLConsts.MinTOOLTYIDLength)]
        public string TOOLTYID { get; set; }

        [Required]
        [StringLength(MFWCTOLConsts.MaxTOOLTYDESCLength, MinimumLength = MFWCTOLConsts.MinTOOLTYDESCLength)]
        public string TOOLTYDESC { get; set; }

        [StringLength(MFWCTOLConsts.MaxUOMLength, MinimumLength = MFWCTOLConsts.MinUOMLength)]
        public string UOM { get; set; }

        public double? REQQTY { get; set; }

        public double? UNITCOST { get; set; }

        public double? TOTALCOST { get; set; }

    }
}