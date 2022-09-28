using System.Collections.Generic;
using ERP.Dto;
using ERP.SupplyChain.Inventory.IC_Segment1.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment1.Exporting
{
    public interface IICSegment1ExcelExporter
    {
        FileDto ExportToFile(List<GetICSegment1ForViewDto> icSegment1s);
    }
}