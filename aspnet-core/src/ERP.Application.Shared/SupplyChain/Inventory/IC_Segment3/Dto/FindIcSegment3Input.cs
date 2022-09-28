using ERP.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.IC_Segment3.Dto
{
    public class FindIcSegment3Input: PagedAndFilteredInputDto
    {
        public string Seg2ID { get; set; }
    }
}
