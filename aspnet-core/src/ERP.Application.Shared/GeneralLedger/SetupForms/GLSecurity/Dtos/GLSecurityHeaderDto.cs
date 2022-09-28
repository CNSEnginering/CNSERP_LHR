
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.GLSecurity.Dtos
{
    public class GLSecurityHeaderDto : EntityDto
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string AudtUser { get; set; }
        public DateTime? AudtDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}