
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.GLSecurity.Dtos
{
    public class GLSecurityDetailDto : EntityDto
    {

        public int DetID { get; set; }
        public string UserID { get; set; }
        public bool CanSee { get; set; }
        public string BeginAcc { get; set; }
        public string EndAcc { get; set; }
        public string AudtUser { get; set; }
        public DateTime? AudtDate { get; set; }

    }
}