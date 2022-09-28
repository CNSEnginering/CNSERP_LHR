using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Sales.SaleEntry.Dtos
{
    public class SaleEntryDto
    {
        public CreateOrEditOESALEHeaderDto OESALEHeader { get; set; }

        public ICollection<CreateOrEditOESALEDetailDto> OESALEDetail { get; set; }
    }
}
