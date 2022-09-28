using System;
using Abp;

namespace ERP.GeneralLedger.SetupForms.ControlDetails.Dto
{
    public class ImportControlDetailFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
    }
}
