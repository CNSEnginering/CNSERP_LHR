using System;
using Abp;

namespace ERP.SupplyChain.Inventory.ItemPrice.Dto
{
    public class ImportItemPriceFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
    }
}
