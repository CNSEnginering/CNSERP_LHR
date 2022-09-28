using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class CreateOrEditMFTOOLDto : EntityDto<int?>
    {

        public int? TenantId { get; set; }

        [Required]
        [StringLength(MFTOOLConsts.MaxTOOLIDLength, MinimumLength = MFTOOLConsts.MinTOOLIDLength)]
        public string TOOLID { get; set; }

        [Required]
        [StringLength(MFTOOLConsts.MaxTOOLDESCLength, MinimumLength = MFTOOLConsts.MinTOOLDESCLength)]
        public string TOOLDESC { get; set; }

        [Required]
        public bool STATUS { get; set; }

        [Required]
        [StringLength(MFTOOLConsts.MaxTOOLTYIDLength, MinimumLength = MFTOOLConsts.MinTOOLTYIDLength)]
        public string TOOLTYID { get; set; }

        [StringLength(MFTOOLConsts.MaxAudtUserLength, MinimumLength = MFTOOLConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        [StringLength(MFTOOLConsts.MaxCreatedByLength, MinimumLength = MFTOOLConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }
        public string tooltypedesc { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}