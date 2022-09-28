using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Manufacturing.SetupForms.Exporting
{
    public class MFTOOLTYExcelExporter : EpPlusExcelExporterBase, IMFTOOLTYExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MFTOOLTYExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMFTOOLTYForViewDto> mftoolty)
        {
            return CreateExcelPackage(
                "MFTOOLTY.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("MFTOOLTY"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("TOOLTYID"),
                        L("TOOLTYDESC"),
                        L("STATUS"),
                        L("UNITCOST"),
                        L("UNIT"),
                        L("COMMENTS"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, mftoolty,
                        _ => _.MFTOOLTY.TOOLTYID,
                        _ => _.MFTOOLTY.TOOLTYDESC,
                        _ => _.MFTOOLTY.STATUS,
                        _ => _.MFTOOLTY.UNITCOST,
                        _ => _.MFTOOLTY.UNIT,
                        _ => _.MFTOOLTY.COMMENTS,
                        _ => _.MFTOOLTY.AudtUser,
                        _ => _timeZoneConverter.Convert(_.MFTOOLTY.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.MFTOOLTY.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.MFTOOLTY.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

                    var audtDateColumn = sheet.Column(8);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    audtDateColumn.AutoFit();
                    var createDateColumn = sheet.Column(10);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    createDateColumn.AutoFit();

                });
        }
    }
}