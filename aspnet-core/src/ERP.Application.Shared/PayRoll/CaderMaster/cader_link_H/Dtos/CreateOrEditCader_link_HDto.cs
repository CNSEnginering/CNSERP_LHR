using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ERP.PayRoll.CaderMaster.cader_link_D.Dtos;
using System.Collections.Generic;

namespace ERP.PayRoll.CaderMaster.cader_link_H.Dtos
{
    public class CreateOrEditCader_link_HDto : EntityDto<int?>
    {

        [StringLength(Cader_link_HConsts.MaxCreatedbyLength, MinimumLength = Cader_link_HConsts.MinCreatedbyLength)]
        public string Createdby { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? Auditdate { get; set; }

        [StringLength(Cader_link_HConsts.MaxAudit_byLength, MinimumLength = Cader_link_HConsts.MinAudit_byLength)]
        public string Audit_by { get; set; }

        public List<Cader_link_DDto> CaderDetail { get; set; }

    }
}