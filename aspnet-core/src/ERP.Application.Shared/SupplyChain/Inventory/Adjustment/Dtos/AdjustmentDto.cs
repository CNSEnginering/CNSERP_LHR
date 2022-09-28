using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.Adjustment.Dtos
{
    public class AdjustmentDto
    {
        public CreateOrEditICADJHeaderDto ICADJHeader { get; set; }

        public ICollection<CreateOrEditICADJDetailDto> ICADJDetail { get; set; }
    }
}
