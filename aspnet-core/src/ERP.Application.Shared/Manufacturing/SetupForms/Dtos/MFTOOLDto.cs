using System;
using Abp.Application.Services.Dto;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class MFTOOLDto : EntityDto
    {
        public int? TenantId { get; set; }

        public string TOOLID { get; set; }

        public string TOOLDESC { get; set; }

        public bool STATUS { get; set; }

        public string TOOLTYID { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}