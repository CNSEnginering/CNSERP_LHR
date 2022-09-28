using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Purchase.ReceiptEntry.Dtos
{
    public class ReceiptEntryDto
    {
        public CreateOrEditPORECHeaderDto PORECHeader { get; set; }

        public ICollection<CreateOrEditPORECDetailDto> PORECDetail { get; set; }

        public ICollection<CreateOrEditICRECAExpDto> ICRECAExp { get; set; }
    }
}
