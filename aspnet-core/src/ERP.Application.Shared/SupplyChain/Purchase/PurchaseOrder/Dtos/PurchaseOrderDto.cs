using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Purchase.PurchaseOrder.Dtos
{
    public class PurchaseOrderDto
    {
        public CreateOrEditPOPOHeaderDto POPOHeader { get; set; }

        public ICollection<CreateOrEditPOPODetailDto> POPODetail { get; set; }
    }
}
