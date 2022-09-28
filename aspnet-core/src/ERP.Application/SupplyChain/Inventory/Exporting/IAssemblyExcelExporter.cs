using System.Collections.Generic;
using ERP.CommonServices.Dtos;
using ERP.Dto;
using ERP.SupplyChain.Inventory.Dtos;

namespace ERP.SupplyChain.Inventory.Exporting
{
    public interface IAssemblyExcelExporter
    {
        FileDto ExportToFile(List<GetAssemblyForViewDto> assembly);
    }
}