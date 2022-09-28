using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.SetupForms.GLSecurity.Dtos
{
    public class GetAllGLSecurityHeaderForExcelInput
    {
        public string Filter { get; set; }

        public string UserIDFilter { get; set; }
        public string UserNameFilter { get; set; }
        public string AudtUserFilter { get; set; }
        public DateTime? MaxAudtDateFilter { get; set; }

        public DateTime? MinAudtDateFilter { get; set; }
        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreatedDateFilter { get; set; }

        public DateTime? MinCreatedDateFilter { get; set; }
    }
}
