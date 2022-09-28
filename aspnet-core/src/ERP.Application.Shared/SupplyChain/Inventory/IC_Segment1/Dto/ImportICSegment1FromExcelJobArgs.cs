using System;
using Abp;

namespace ERP.SupplyChain.Inventory.IC_Segment1.Dto
{
    public class ImportICSegment1FromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
    }
}
