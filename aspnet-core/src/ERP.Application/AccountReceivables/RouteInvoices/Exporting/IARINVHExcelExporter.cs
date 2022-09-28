using System.Collections.Generic;
using ERP.AccountReceivables.RouteInvoices.Dtos;
using ERP.Dto;

namespace ERP.AccountReceivables.RouteInvoices.Exporting
{
    public interface IARINVHExcelExporter
    {
        FileDto ExportToFile(List<GetARINVHForViewDto> arinvh);
    }
}