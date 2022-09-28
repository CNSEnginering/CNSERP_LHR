using System.Collections.Generic;
using ERP.CommonServices.ChequeBooks.Dtos;
using ERP.Dto;

namespace ERP.CommonServices.ChequeBooks.Exporting
{
    public interface IChequeBookDetailsExcelExporter
    {
        FileDto ExportToFile(List<GetChequeBookDetailForViewDto> chequeBookDetails);
    }
}