using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.SetupForms.GLSecurity.Dtos
{
    public class GetGLSecurityDetailForEditOutput
    {
        public ICollection<CreateOrEditGLSecurityDetailDto> GLSecurityDetail { get; set; }
    }
}
