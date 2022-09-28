using System.Collections.Generic;
using ERP.SupplyChain.Inventory.ICOPT4.Dtos;
using ERP.Dto;
using ERP.SupplyChain.Inventory.IC_Item.Dto;

namespace ERP.SupplyChain.Inventory.IC_Item.Exporting
{
    public interface IICITEMExcelExporter
    {
        FileDto ExportToFile(List<GetIcItemForViewDto> icopT4);
    }
}