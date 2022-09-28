using System.Collections.Generic;
using ERP.SupplyChain.Sales.OECSH.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Sales.OECSH.Exporting
{
    public interface IOECSHExcelExporter
    {
        FileDto ExportToFile(List<GetOECSHForViewDto> oecsh);
    }
}