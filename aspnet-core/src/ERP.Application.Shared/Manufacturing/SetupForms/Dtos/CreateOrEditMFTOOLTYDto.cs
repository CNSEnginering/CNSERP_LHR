using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class CreateOrEditMFTOOLTYDto : EntityDto<int?>
    {

        [Required]
        [StringLength(MFTOOLTYConsts.MaxTOOLTYIDLength, MinimumLength = MFTOOLTYConsts.MinTOOLTYIDLength)]
        public string TOOLTYID { get; set; }

        [Required]
        [StringLength(MFTOOLTYConsts.MaxTOOLTYDESCLength, MinimumLength = MFTOOLTYConsts.MinTOOLTYDESCLength)]
        public string TOOLTYDESC { get; set; }

        [Required]
        public bool STATUS { get; set; }

        [Required]
        public double UNITCOST { get; set; }

        [Required]
        [StringLength(MFTOOLTYConsts.MaxUNITLength, MinimumLength = MFTOOLTYConsts.MinUNITLength)]
        public string UNIT { get; set; }

        [Required]
        [StringLength(MFTOOLTYConsts.MaxCOMMENTSLength, MinimumLength = MFTOOLTYConsts.MinCOMMENTSLength)]
        public string COMMENTS { get; set; }

        [StringLength(MFTOOLTYConsts.MaxAudtUserLength, MinimumLength = MFTOOLTYConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        [StringLength(MFTOOLTYConsts.MaxCreatedByLength, MinimumLength = MFTOOLTYConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}