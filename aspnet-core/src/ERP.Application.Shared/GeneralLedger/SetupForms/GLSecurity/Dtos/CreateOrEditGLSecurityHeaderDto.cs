using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.GeneralLedger.SetupForms.GLSecurity.Dtos
{
    public class CreateOrEditGLSecurityHeaderDto: EntityDto<int?>
    {
        [Required]
        public string UserID { get; set; }
        [Required]
        public string UserName { get; set; }
        public string AudtUser { get; set; }
        public DateTime? AudtDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public bool flag { get; set; }

        public ICollection<CreateOrEditGLSecurityDetailDto> GLSecurityDetail { get; set; }

    }
}
