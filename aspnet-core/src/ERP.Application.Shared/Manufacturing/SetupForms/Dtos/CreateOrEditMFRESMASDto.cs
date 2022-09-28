using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class CreateOrEditMFRESMASDto : EntityDto<int?>
    {

        [Required]
        [StringLength(MFRESMASConsts.MaxRESIDLength, MinimumLength = MFRESMASConsts.MinRESIDLength)]
        public string RESID { get; set; }

        [Required]
        [StringLength(MFRESMASConsts.MaxRESDESCLength, MinimumLength = MFRESMASConsts.MinRESDESCLength)]
        public string RESDESC { get; set; }

        [Required]
        public bool ACTIVE { get; set; }

        [Required]
        public short COSTTYPE { get; set; }

        [Required]
        public decimal UNITCOST { get; set; }

        [Required]
        public short UOMTYPE { get; set; }
        [Required]
        public short COSTBASIS { get; set; }

        [Required]
        [StringLength(MFRESMASConsts.MaxUNITLength, MinimumLength = MFRESMASConsts.MinUNITLength)]
        public string UNIT { get; set; }

        [StringLength(MFRESMASConsts.MaxAudtUserLength, MinimumLength = MFRESMASConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        [StringLength(MFRESMASConsts.MaxCreatedByLength, MinimumLength = MFRESMASConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}