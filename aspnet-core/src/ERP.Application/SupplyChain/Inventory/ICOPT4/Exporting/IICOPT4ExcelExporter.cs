using System.Collections.Generic;
using ERP.SupplyChain.Inventory.ICOPT4.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory.ICOPT4.Exporting
{
    public interface IICOPT4ExcelExporter
    {
        FileDto ExportToFile(List<GetICOPT4ForViewDto> icopT4);
    }
}