using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.Exporting
{
    public class SubControlDetailsExcelExporter : EpPlusExcelExporterBase, ISubControlDetailsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SubControlDetailsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSubControlDetailForViewDto> subControlDetails)
        {
            return CreateExcelPackage(
                "SubControlDetails.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("SubControlDetails"));
                    sheet.OutLineApplyStyle = true;

                AddHeader(
                    sheet,

                    L("Seg1ID"),
                    L("SegmentName"),
                        L("Seg2ID"),
                        L("SegmentName2"),
                        L("OldCode")
                        
                        );

                    AddObjects(
                        sheet, 2, subControlDetails,
                         _ => _.ControlDetailId,
                         _=> _.ControlDetaildesc,
                          _ => _.SubControlDetail.Seg2ID,
                        _ => _.SubControlDetail.SegmentName,
                        _ => _.SubControlDetail.OldCode
                        );

					

                });
        }
    }
}
