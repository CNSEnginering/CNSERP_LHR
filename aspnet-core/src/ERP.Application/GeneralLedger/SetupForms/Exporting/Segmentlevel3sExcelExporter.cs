using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.Exporting
{
    public class Segmentlevel3sExcelExporter : EpPlusExcelExporterBase, ISegmentlevel3sExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public Segmentlevel3sExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSegmentlevel3ForViewDto> segmentlevel3s)
        {
            return CreateExcelPackage(
                "Segmentlevel3s.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Segmentlevel3s"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Seg1ID"),
                        L("SegmentName"),
                        L("Seg2ID"),
                        L("SegmentName2"),
                        L("Seg3ID"),
                        L("Segmentlevel3"),
                        L("OldCode")
                        
                        );

                    AddObjects(
                        sheet, 2, segmentlevel3s,
                        _ => _.ControlDetailId,
                        _ => _.ControlDetailDesc,
                        _ => _.SubControlDetailId,
                        _ => _.SubControlDetailDesc,
                        _ => _.Segmentlevel3.Seg3ID,
                        _ => _.Segmentlevel3.SegmentName,
                        _ => _.Segmentlevel3.OldCode
                        );

					

                });
        }
    }
}
