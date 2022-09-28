using System.Collections.Generic;
using ERP.SupplyChain.Inventory.ICOPT5.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory.ICOPT5.Exporting
{
    public interface IICOPT5ExcelExporter
    {
        FileDto ExportToFile(List<GetICOPT5ForViewDto> icopT5);
    }
}