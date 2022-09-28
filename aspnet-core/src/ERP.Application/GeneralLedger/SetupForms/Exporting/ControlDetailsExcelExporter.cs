using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.Exporting
{
    public class ControlDetailsExcelExporter : EpPlusExcelExporterBase, IControlDetailsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ControlDetailsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetControlDetailForViewDto> controlDetails)
        {
            return CreateExcelPackage(
                "ControlDetails.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ControlDetails"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Seg1ID"),
                        L("SegmentName"),
                        (L("GroupCategory")) + L("Id"),
                       (L("GroupCategory")),
                        L("OldCode")
                        );

                    AddObjects(
                        sheet, 2, controlDetails,
                        _ => _.ControlDetail.Seg1ID,
                        _ => _.ControlDetail.SegmentName,
                        _ => _.ControlDetail.Family,
                         _ => _.FamilyDesc,
                        _ => _.ControlDetail.OldCode
                       
                        );

					

                });
        }
    }
}
