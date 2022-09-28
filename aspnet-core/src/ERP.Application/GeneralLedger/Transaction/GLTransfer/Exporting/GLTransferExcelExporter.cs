using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.GeneralLedger.Transaction.GLTransfer.Dtos;
using ERP.Storage;
using System.Collections.Generic;

namespace ERP.GeneralLedger.Transaction.GLTransfer.Exporting
{
    public class GLTransferExcelExporter: EpPlusExcelExporterBase, IGLTransferExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public GLTransferExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
        base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetGLTransferForViewDto> GLTransfer)
        {
            return CreateExcelPackage(
                "GLTransfer.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("GLTransfer"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DOCID"),
                        L("DOCDATE"),
                        L("TRANSFERDATE"),
                        L("DESCRIPTION"),
                        L("FROMLOCID"),
                        L("FROMBANKID"),
                        L("FROMCONFIGID"),            
                        L("FROMBANKACCID"),
                        L("FROMACCID"),
                        L("TOLOCID"),
                        L("TOBANKID"),
                        L("TOCONFIGID"),
                        L("TOBANKACCID"),
                        L("TOACCID"),
                        L("TRANSFERAMOUNT"),
                        L("STATUS"),
                        L("AUDTUSER"),
                        L("AUDTDATE"),
                        L("CreatedBy"),
                        L("CreatedOn")
                        );

                    AddObjects(
                        sheet, 2, GLTransfer,
                        _ => _.GLTransfer.DOCID,
                        _ => _timeZoneConverter.Convert(_.GLTransfer.DOCDATE, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.GLTransfer.TRANSFERDATE, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.GLTransfer.DESCRIPTION,
                        _ => _.GLTransfer.FROMLOCID,
                        _ => _.GLTransfer.FROMBANKID,
                        _ => _.GLTransfer.FROMCONFIGID,
                        _ => _.GLTransfer.FROMBANKACCID,
                        _ => _.GLTransfer.FROMACCID,
                        _ => _.GLTransfer.TOLOCID,
                        _ => _.GLTransfer.TOBANKID,
                        _ => _.GLTransfer.TOBANKID,
                        _ => _.GLTransfer.TOCONFIGID,
                        _ => _.GLTransfer.TOBANKACCID,
                        _ => _.GLTransfer.TOACCID,
                        _ => _.GLTransfer.TRANSFERAMOUNT,
                        _ => _.GLTransfer.STATUS,
                        _ => _.GLTransfer.AUDTUSER,
                        _ => _timeZoneConverter.Convert(_.GLTransfer.AUDTDATE, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.GLTransfer.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.GLTransfer.CreatedOn, _abpSession.TenantId, _abpSession.GetUserId())
                        );

                    var docDateColumn = sheet.Column(2);
                    docDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    docDateColumn.AutoFit();
                    var transferDateColumn = sheet.Column(3);
                    transferDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    transferDateColumn.AutoFit();
                    var audtDateColumn = sheet.Column(19);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    audtDateColumn.AutoFit();
                    var createDateColumn = sheet.Column(21);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    createDateColumn.AutoFit();


                });
        }
    }
}
