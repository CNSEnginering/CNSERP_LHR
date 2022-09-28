using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.WorkOrder.Dtos
{
    public class WorkOrderDto
    {
        public CreateOrEditICWOHeaderDto ICWOHeader { get; set; }

        public ICollection<CreateOrEditICWODetailDto> ICWODetail { get; set; }
    }
}
