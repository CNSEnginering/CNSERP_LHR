using System.Collections.Generic;
using ERP.SupplyChain.Sales.SaleQutation.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Sales.SaleQutation.Exporting
{
    public interface IOEQHExcelExporter
    {
        FileDto ExportToFile(List<GetOEQHForViewDto> oeqh);
    }
}