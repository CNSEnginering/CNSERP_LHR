using System;
using Abp.Application.Services.Dto;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class MFACSETDto : EntityDto
    {
        public int? TenantId { get; set; }

        public string ACCTSET { get; set; }

        public string DESC { get; set; }

        public string WIPACCT { get; set; }

        public string SETLABACCT { get; set; }

        public string RUNLABACCT { get; set; }

        public string OVHACCT { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string wipaccDesc { get; set; }
        public string labaccDesc { get; set; }
        public string runLabAccDesc { get; set; }
        public string ovhacctDesc { get; set; }

    }
}