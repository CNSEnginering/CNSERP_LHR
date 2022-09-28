using System.Collections.Generic;
using ERP.Dto;
using ERP.GeneralLedger.SetupForms.Importing.ChartofAccount.Dto;
using ERP.GeneralLedger.SetupForms.Importing.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.ChartofAccount
{
    public interface IInvalidChartofAccountExporter
    {
        FileDto ExportToFile(List<ImportChartofAccountDto> userListDtos);
    }
}
