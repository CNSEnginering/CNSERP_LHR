using System.Collections.Generic;
using ERP.AccountReceivables.RouteInvoices.Dtos;
using ERP.Dto;

namespace ERP.AccountReceivables.RouteInvoices.Exporting
{
    public interface IARINVDExcelExporter
    {
        FileDto ExportToFile(List<GetARINVDForViewDto> arinvd);
    }
}