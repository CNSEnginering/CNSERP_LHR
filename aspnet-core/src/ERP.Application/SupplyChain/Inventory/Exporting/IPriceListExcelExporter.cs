using ERP.Dto;
using ERP.SupplyChain.Inventory.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.Exporting
{
    public interface IPriceListExcelExporter
    {
        FileDto ExportToFile(List<GetPriceListForViewDto> priceList);
    }
}
