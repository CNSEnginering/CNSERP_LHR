using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.CaderMaster.cader_link_H.Dtos
{
    public class Cader_link_HDto : EntityDto
    {
        public string Createdby { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? Auditdate { get; set; }

        public string Audit_by { get; set; }

    }
}