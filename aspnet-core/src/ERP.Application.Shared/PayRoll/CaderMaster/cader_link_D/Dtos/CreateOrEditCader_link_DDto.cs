using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.CaderMaster.cader_link_D.Dtos
{
    public class CreateOrEditCader_link_DDto : EntityDto<int?>
    {

        [Required]
        public int CaderID { get; set; }

        [Required]
        [StringLength(Cader_link_DConsts.MaxAccountIDLength, MinimumLength = Cader_link_DConsts.MinAccountIDLength)]
        public string AccountID { get; set; }

        [StringLength(Cader_link_DConsts.MaxAccountNameLength, MinimumLength = Cader_link_DConsts.MinAccountNameLength)]
        public string AccountName { get; set; }

        [Required]
        [StringLength(Cader_link_DConsts.MaxTypeLength, MinimumLength = Cader_link_DConsts.MinTypeLength)]
        public string Type { get; set; }

        [Required]
        [StringLength(Cader_link_DConsts.MaxPayTypeLength, MinimumLength = Cader_link_DConsts.MinPayTypeLength)]
        public string PayType { get; set; }

        [StringLength(Cader_link_DConsts.MaxNarrationLength, MinimumLength = Cader_link_DConsts.MinNarrationLength)]
        public string Narration { get; set; }

    }
}