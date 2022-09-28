using System.Collections.Generic;
using ERP.AccountPayables.Dtos;
using ERP.Dto;

namespace ERP.AccountPayables.Exporting
{
    public interface IAPTermsExcelExporter
    {
        FileDto ExportToFile(List<GetAPTermForViewDto> apTerms);
    }
}