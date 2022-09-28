using System;
using Abp;

namespace ERP.SupplyChain.Inventory.IC_Segment3.Dto
{
    public class ImportICSegment3FromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
    }
}
