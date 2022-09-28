using System.Collections.Generic;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms.Exporting
{
    public interface ISegmentlevel3sExcelExporter
    {
        FileDto ExportToFile(List<GetSegmentlevel3ForViewDto> segmentlevel3s);
    }
}