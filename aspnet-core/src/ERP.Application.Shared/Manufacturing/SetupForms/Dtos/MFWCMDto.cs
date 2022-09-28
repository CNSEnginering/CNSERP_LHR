using System;
using Abp.Application.Services.Dto;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class MFWCMDto : EntityDto
    {
        public string WCID { get; set; }

        public string WCESC { get; set; }

        public double? TOTRSCCOST { get; set; }

        public double? TOTTLCOST { get; set; }

        public string COMMENTS { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}