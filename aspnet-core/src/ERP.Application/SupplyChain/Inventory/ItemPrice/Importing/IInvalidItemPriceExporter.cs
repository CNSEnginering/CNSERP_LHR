using System.Collections.Generic;
using ERP.Dto;
using ERP.SupplyChain.Inventory.ItemPrice.Importing.Dto;

namespace ERP.SupplyChain.Inventory.ItemPrice.Importing
{
    public interface IInvalidItemPriceExporter
    {
        FileDto ExportToFile(List<ImportItemPriceDto> userListDtos);
    }
}
