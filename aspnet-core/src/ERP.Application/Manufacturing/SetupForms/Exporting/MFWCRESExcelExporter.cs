using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Manufacturing.SetupForms.Exporting
{
    public class MFWCRESExcelExporter : EpPlusExcelExporterBase, IMFWCRESExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MFWCRESExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMFWCRESForViewDto> mfwcres)
        {
            return CreateExcelPackage(
                "MFWCRES.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("MFWCRES"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DetID"),
                        L("WCID"),
                        L("RESID"),
                        L("RESDESC"),
                        L("UOM"),
                        L("REQQTY"),
                        L("UNITCOST"),
                        L("TOTALCOST")
                        );

                    AddObjects(
                        sheet, 2, mfwcres,
                        _ => _.MFWCRES.DetID,
                        _ => _.MFWCRES.WCID,
                        _ => _.MFWCRES.RESID,
                        _ => _.MFWCRES.RESDESC,
                        _ => _.MFWCRES.UOM,
                        _ => _.MFWCRES.REQQTY,
                        _ => _.MFWCRES.UNITCOST,
                        _ => _.MFWCRES.TOTALCOST
                        );

                });
        }
    }
}