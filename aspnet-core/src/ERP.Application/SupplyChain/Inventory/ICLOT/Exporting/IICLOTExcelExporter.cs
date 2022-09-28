using System.Collections.Generic;
using ERP.SupplyChain.Inventory.ICLOT.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory.ICLOT.Exporting
{
    public interface IICLOTExcelExporter
    {
        FileDto ExportToFile(List<GetICLOTForViewDto> iclot);
    }
}