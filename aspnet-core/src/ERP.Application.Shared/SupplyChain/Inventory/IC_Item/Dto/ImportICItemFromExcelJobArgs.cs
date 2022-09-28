using System;
using Abp;

namespace ERP.SupplyChain.Inventory.IC_Item.Dto
{
    public class ImportICItemFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
    }
}
