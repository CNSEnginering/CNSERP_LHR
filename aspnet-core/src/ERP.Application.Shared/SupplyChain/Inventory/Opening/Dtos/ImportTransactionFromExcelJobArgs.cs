using Abp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.Opening.Dtos
{
    public class ImportTransactionFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
    }
}
