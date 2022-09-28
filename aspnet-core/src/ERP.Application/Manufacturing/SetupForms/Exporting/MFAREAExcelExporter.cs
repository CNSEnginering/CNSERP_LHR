using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Manufacturing.SetupForms.Exporting
{
    public class MFAREAExcelExporter : EpPlusExcelExporterBase, IMFAREAExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MFAREAExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMFAREAForViewDto> mfarea)
        {
            return CreateExcelPackage(
                "MFAREA.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("MFAREA"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("AREAID"),
                        L("AREADESC"),
                        L("AREATY"),
                        L("STATUS"),
                        L("ADDRESS"),
                        L("CONTNAME"),
                        L("CONTPOS"),
                        L("CONTCELL"),
                        L("CONTEMAIL"),
                        L("LOCID"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, mfarea,
                        _ => _.MFAREA.AREAID,
                        _ => _.MFAREA.AREADESC,
                        _ => _.MFAREA.AREATY,
                        _ => _.MFAREA.STATUS,
                        _ => _.MFAREA.ADDRESS,
                        _ => _.MFAREA.CONTNAME,
                        _ => _.MFAREA.CONTPOS,
                        _ => _.MFAREA.CONTCELL,
                        _ => _.MFAREA.CONTEMAIL,
                        _ => _.MFAREA.LOCID,
                        _ => _.MFAREA.AudtUser,
                        _ => _timeZoneConverter.Convert(_.MFAREA.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.MFAREA.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.MFAREA.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

                    var audtDateColumn = sheet.Column(12);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    audtDateColumn.AutoFit();
                    var createDateColumn = sheet.Column(14);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    createDateColumn.AutoFit();

                });
        }
    }
}