using System.Collections.Generic;
using ERP.Dto;
using ERP.SupplyChain.Inventory.IC_Item.Importing.Dto;

namespace ERP.SupplyChain.Inventory.IC_Item.Importing
{
    public interface IInvalidICItemExporter
    {
        FileDto ExportToFile(List<ImportICItemDto> userListDtos);
    }
}
