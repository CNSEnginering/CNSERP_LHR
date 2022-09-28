using System.Collections.Generic;
using ERP.AccountPayables.Dtos;
using ERP.Dto;

namespace ERP.AccountPayables.Exporting
{
    public interface IAPOptionsExcelExporter
    {
        FileDto ExportToFile(List<GetAPOptionForViewDto> apOptions);
    }
}