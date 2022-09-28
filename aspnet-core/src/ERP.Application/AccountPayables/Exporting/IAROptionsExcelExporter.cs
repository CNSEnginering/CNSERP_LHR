using System.Collections.Generic;
using ERP.AccountPayables.Dtos;
using ERP.Dto;

namespace ERP.AccountPayables.Exporting
{
    public interface IAROptionsExcelExporter
    {
        FileDto ExportToFile(List<GetAROptionForViewDto> arOptions);
    }
}