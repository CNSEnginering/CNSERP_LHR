using System.Collections.Generic;
using ERP.SupplyChain.Inventory.IC_Segment2.Dto;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment2.Exporting
{
    public interface IICSegment2ExcelExporter
    {
        FileDto ExportToFile(List<GetICSegment2ForViewDto> icSegment2);
    }
}