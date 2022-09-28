using ERP.Dto;
using ERP.GeneralLedger.Transaction.GLTransfer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.Transaction.GLTransfer.Exporting
{
    public interface IGLTransferExcelExporter
    {
        FileDto ExportToFile(List<GetGLTransferForViewDto> GLTransfer);
    }
}
