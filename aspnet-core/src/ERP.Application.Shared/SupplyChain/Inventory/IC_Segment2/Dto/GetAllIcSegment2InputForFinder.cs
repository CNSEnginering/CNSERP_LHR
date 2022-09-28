using ERP.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.IC_Segment2.Dto
{
    public class GetAllIcSegment2InputForFinder : PagedAndFilteredInputDto
    {
      public string seg1ID { get; set; }
    }
}
