using System.Collections.Generic;
using ERP.GeneralLedger.Transaction.BankReconcile.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.Transaction.BankReconcile.Exporting
{
    public interface IBankReconcilesExcelExporter
    {
        FileDto ExportToFile(List<GetBankReconcileForViewDto> bankReconciles);
    }
}