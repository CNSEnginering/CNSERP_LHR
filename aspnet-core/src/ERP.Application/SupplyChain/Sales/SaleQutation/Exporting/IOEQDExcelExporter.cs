using System.Collections.Generic;
using ERP.SupplyChain.Sales.SaleQutation.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Sales.SaleQutation.Exporting
{
    public interface IOEQDExcelExporter
    {
        FileDto ExportToFile(List<GetOEQDForViewDto> oeqd);
    }
}