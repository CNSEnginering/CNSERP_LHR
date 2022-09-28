using System.Collections.Generic;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms.Exporting
{
    public interface IChartofControlsExcelExporter
    {
        FileDto ExportToFile(List<GetChartofControlForViewDto> chartofControls);
    }
}