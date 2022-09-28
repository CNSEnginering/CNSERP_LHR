using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.Consumption.Dtos
{
    public class ConsumptionDto
    {
        public CreateOrEditICCNSHeaderDto ICCNSHeader { get; set; }

        public ICollection<CreateOrEditICCNSDetailDto> ICCNSDetail { get; set; }
    }
}
