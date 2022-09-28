using System;
using Abp;

namespace ERP.GeneralLedger.SetupForms.GroupCodes.Dto
{
    public class ImportGroupCodesFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
    }
}
