using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.GeneralLedger.SetupForms.GLSecurity.Dtos
{
    public class CreateOrEditGLSecurityDetailDto: EntityDto<int?>
    {
        [Required]
        public int DetID { get; set; }
        public string UserID { get; set; }
        public bool CanSee { get; set; }
        public string BeginAcc { get; set; }
        public string EndAcc { get; set; }
        public string AudtUser { get; set; }
        public DateTime? AudtDate { get; set; }

    }
}
