using Abp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.AccountPayables.Dtos
{
    public class ImportDirectInvoiceFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
    }
}
