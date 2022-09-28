using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.GLDocRev.Dtos
{
    public class CreateOrEditGLDocRevDto : EntityDto<int?>
    {

        [StringLength(GLDocRevConsts.MaxBookIDLength, MinimumLength = GLDocRevConsts.MinBookIDLength)]
        public string BookID { get; set; }

        public int? DocNo { get; set; }
        public int? MaxDocNo { get; set; }
        public DateTime? DocDate { get; set; }
        public  int? DetId { get; set; }
        public int? FmtDocNo { get; set; }

        public int? DocYear { get; set; }

        public int? DocMonth { get; set; }

        public DateTime? NewDocDate { get; set; }

        public int? NewDocNo { get; set; }

        public int? NewFmtDocNo { get; set; }

        public int? NewDocYear { get; set; }

        public int? NewDocMonth { get; set; }

        [StringLength(GLDocRevConsts.MaxNarrationLength, MinimumLength = GLDocRevConsts.MinNarrationLength)]
        public string Narration { get; set; }

        [Required]
        public bool Posted { get; set; }

        [StringLength(GLDocRevConsts.MaxPostedByLength, MinimumLength = GLDocRevConsts.MinPostedByLength)]
        public string PostedBy { get; set; }

        public DateTime? PostedDate { get; set; }

    }
}