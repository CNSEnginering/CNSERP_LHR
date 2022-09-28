using ERP.Dto;
using ERP.SupplyChain.Inventory.IC_Segment3.Dto;
using System.Collections.Generic;


namespace ERP.SupplyChain.Inventory.IC_Segment3.Exporting
{
    public interface IICSegment3ExcelExporter
    {
        FileDto ExportToFile(List<GetICSegment3ForViewDto> icSegment3);
    }
}