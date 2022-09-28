using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.Opening.Dtos
{
    public class OpeningDto
    {
        public CreateOrEditICOPNHeaderDto ICOPNHeader { get; set; }

        public ICollection<CreateOrEditICOPNDetailDto> ICOPNDetail { get; set; }
    }
}
