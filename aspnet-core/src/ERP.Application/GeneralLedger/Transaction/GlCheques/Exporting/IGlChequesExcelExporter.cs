using System.Collections.Generic;
using ERP.GeneralLedger.Transaction.BankReconcile.Dtos;
using ERP.Dto;
using ERP.GeneralLedger.Transaction.Dtos;

namespace ERP.GeneralLedger.Transaction.BankReconcile.Exporting
{
    public interface IGlChequesExcelExporter
    {
        FileDto ExportToFile(List<GetGlChequeForViewDto> glCheques);
    }
}