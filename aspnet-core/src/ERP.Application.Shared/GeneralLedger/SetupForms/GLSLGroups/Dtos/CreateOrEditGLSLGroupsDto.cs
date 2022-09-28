using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.GLSLGroups.Dtos
{
    public class CreateOrEditGLSLGroupsDto : EntityDto<int?>
    {

        [Required]
        public int SLGrpID { get; set; }

        [StringLength(GLSLGroupsConsts.MaxSLGRPDESCLength, MinimumLength = GLSLGroupsConsts.MinSLGRPDESCLength)]
        public string SLGRPDESC { get; set; }

        public bool Active { get; set; }

        [StringLength(GLSLGroupsConsts.MaxAudtUserLength, MinimumLength = GLSLGroupsConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        [StringLength(GLSLGroupsConsts.MaxCreatedByLength, MinimumLength = GLSLGroupsConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}