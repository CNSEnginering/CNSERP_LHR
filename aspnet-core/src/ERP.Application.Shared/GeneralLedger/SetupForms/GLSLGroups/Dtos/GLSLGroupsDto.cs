using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.GLSLGroups.Dtos
{
    public class GLSLGroupsDto : EntityDto
    {
        public int SLGrpID { get; set; }

        public string SLGRPDESC { get; set; }

        public bool Active { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}