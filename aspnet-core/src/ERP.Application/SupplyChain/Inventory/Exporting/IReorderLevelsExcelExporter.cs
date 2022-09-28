using System.Collections.Generic;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory.Exporting
{
    public interface IReorderLevelsExcelExporter
    {
        FileDto ExportToFile(List<ReorderLevelDto> reorderLevels);
    }
}