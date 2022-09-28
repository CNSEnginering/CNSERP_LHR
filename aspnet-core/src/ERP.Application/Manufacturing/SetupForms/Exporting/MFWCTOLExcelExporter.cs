using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Manufacturing.SetupForms.Exporting
{
    public class MFWCTOLExcelExporter : EpPlusExcelExporterBase, IMFWCTOLExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MFWCTOLExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMFWCTOLForViewDto> mfwctol)
        {
            return CreateExcelPackage(
                "MFWCTOL.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("MFWCTOL"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DetID"),
                        L("WCID"),
                        L("TOOLTYID"),
                        L("TOOLTYDESC"),
                        L("UOM"),
                        L("REQQTY"),
                        L("UNITCOST"),
                        L("TOTALCOST")
                        );

                    AddObjects(
                        sheet, 2, mfwctol,
                        _ => _.MFWCTOL.DetID,
                        _ => _.MFWCTOL.WCID,
                        _ => _.MFWCTOL.TOOLTYID,
                        _ => _.MFWCTOL.TOOLTYDESC,
                        _ => _.MFWCTOL.UOM,
                        _ => _.MFWCTOL.REQQTY,
                        _ => _.MFWCTOL.UNITCOST,
                        _ => _.MFWCTOL.TOTALCOST
                        );

                });
        }
    }
}