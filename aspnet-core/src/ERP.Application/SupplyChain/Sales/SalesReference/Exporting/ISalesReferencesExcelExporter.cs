using System.Collections.Generic;
using ERP.SupplyChain.Sales.SalesReference.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Sales.SalesReference.Exporting
{
    public interface ISalesReferencesExcelExporter
    {
        FileDto ExportToFile(List<GetSalesReferenceForViewDto> salesReferences);
    }
}