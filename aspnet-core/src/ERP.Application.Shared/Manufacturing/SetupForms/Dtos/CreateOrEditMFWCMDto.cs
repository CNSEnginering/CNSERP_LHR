using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class CreateOrEditMFWCMDto : EntityDto<int?>
    {

        [Required]
        [StringLength(MFWCMConsts.MaxWCIDLength, MinimumLength = MFWCMConsts.MinWCIDLength)]
        public string WCID { get; set; }

        [Required]
        [StringLength(MFWCMConsts.MaxWCESCLength, MinimumLength = MFWCMConsts.MinWCESCLength)]
        public string WCESC { get; set; }

        public double? TOTRSCCOST { get; set; }

        public double? TOTTLCOST { get; set; }

        [StringLength(MFWCMConsts.MaxCOMMENTSLength, MinimumLength = MFWCMConsts.MinCOMMENTSLength)]
        public string COMMENTS { get; set; }

        [StringLength(MFWCMConsts.MaxAudtUserLength, MinimumLength = MFWCMConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        [StringLength(MFWCMConsts.MaxCreatedByLength, MinimumLength = MFWCMConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public List<CreateOrEditMFWCRESDto> ResDetailDto { get; set; }
        public List<CreateOrEditMFWCTOLDto> ToolDetailDto { get; set; }

    }
}