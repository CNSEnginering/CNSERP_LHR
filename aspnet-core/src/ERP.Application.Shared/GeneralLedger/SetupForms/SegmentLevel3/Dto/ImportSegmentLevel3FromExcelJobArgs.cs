using System;
using Abp;

namespace ERP.GeneralLedger.SetupForms.SegmentLevel3.Dto
{
    public class ImportSegmentLevel3FromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
    }
}
