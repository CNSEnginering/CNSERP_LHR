using System.Collections.Generic;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Exporting
{
    public interface IGLTRHeadersExcelExporter
    {
        FileDto ExportToFile(List<GetGLTRHeaderForViewDto> gltrHeaders);
    }
}