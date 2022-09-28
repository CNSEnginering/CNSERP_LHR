using Abp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Sales.SaleEntry.Dtos
{
    public class ImportSaleEntryFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
    }
}
