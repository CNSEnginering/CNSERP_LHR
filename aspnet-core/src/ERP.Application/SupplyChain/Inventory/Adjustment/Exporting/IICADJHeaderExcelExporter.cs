using System.Collections.Generic;
using ERP.SupplyChain.Inventory.Adjustment.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory.Adjustment.Exporting
{
    public interface IICADJHeaderExcelExporter
    {
        FileDto ExportToFile(List<GetICADJHeaderForViewDto> adjustments);
    }
}