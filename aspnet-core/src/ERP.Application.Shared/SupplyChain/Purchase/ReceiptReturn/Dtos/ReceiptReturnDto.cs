using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Purchase.ReceiptReturn.Dtos
{
    public class ReceiptReturnDto
    {
        public CreateOrEditPORETHeaderDto PORETHeader { get; set; }

        public ICollection<CreateOrEditPORETDetailDto> PORETDetail { get; set; }
    }
}
