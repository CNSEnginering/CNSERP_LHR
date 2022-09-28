using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.Cader.Dtos
{
    public class CreateOrEditCaderDto : EntityDto<long?>
    {

        [StringLength(CaderConsts.MaxCADER_NAMELength, MinimumLength = CaderConsts.MinCADER_NAMELength)]
        public string CADER_NAME { get; set; }

        [StringLength(CaderConsts.MaxAudtUserLength, MinimumLength = CaderConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        [StringLength(CaderConsts.MaxCreatedByLength, MinimumLength = CaderConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}