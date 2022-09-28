using System.Collections.Generic;
using Abp.Dependency;
using ERP.SupplyChain.Inventory.IC_Item.Importing.Dto;

namespace ERP.SupplyChain.Inventory.IC_Item.Importing
{
    public interface IICItemListExcelDataReader : ITransientDependency
    {
        List<ImportICItemDto> GetICItemFromExcel(byte[] fileBytes);
    }
}
