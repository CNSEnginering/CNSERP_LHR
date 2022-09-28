using System.Collections.Generic;
using Abp.Dependency;
using ERP.SupplyChain.Inventory.ItemPrice.Importing.Dto;

namespace ERP.SupplyChain.Inventory.ItemPrice.Importing
{
    public interface IItemPriceListExcelDataReader : ITransientDependency
    {
        List<ImportItemPriceDto> GetItemPriceFromExcel(byte[] fileBytes);
    }
}
