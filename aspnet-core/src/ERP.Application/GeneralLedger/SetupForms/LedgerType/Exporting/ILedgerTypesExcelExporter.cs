using System.Collections.Generic;
using ERP.GeneralLedger.SetupForms.LedgerType.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms.LedgerType.Exporting
{
    public interface ILedgerTypesExcelExporter
    {
        FileDto ExportToFile(List<GetLedgerTypeForViewDto> ledgerTypes);
    }
}