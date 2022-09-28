using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.SetupForms.GLSecurity.Dtos
{
    public class GetAllGLSecurityDetailForExcelInput
    {
        public string Filter { get; set; }
        public int? MaxDetIDFilter { get; set; }
        public int? MinDetIDFilter { get; set; }
        public string UserIDFilter { get; set; }
        public int CanSeeFilter { get; set; }
        public string BeginAccFilter { get; set; }
        public string EndAccFilter { get; set; }
        public string AudtUserFilter { get; set; }
        public DateTime? MaxAudtDateFilter { get; set; }

        public DateTime? MinAudtDateFilter { get; set; }
    }
}
