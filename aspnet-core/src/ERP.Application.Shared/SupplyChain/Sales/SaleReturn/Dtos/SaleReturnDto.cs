using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Sales.SaleReturn.Dtos
{
    public class SaleReturnDto
    {
        public CreateOrEditOERETHeaderDto OERETHeader { get; set; }

        public ICollection<CreateOrEditOERETDetailDto> OERETDetail { get; set; }
    }
}
