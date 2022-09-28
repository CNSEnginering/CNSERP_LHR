using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.AccountPayables.CRDRNote.Dtos;
using ERP.AccountPayables.CRDRNote.Exporting;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.Storage;
using System.Collections.Generic;

namespace ERP.AccountPayables.Exporting
{
    public class CRDRNoteExcelExporter: EpPlusExcelExporterBase, ICRDRNoteExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CRDRNoteExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
        base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCRDRNoteForViewDto> CRDRNote)
        {
            return CreateExcelPackage(
                "CRDRNote.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("CRDRNote"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("LocID"),
                        L("DocNo"),
                        L("DocDate"),
                        L("PostingDate"),
                        L("PaymentDate"),
                        L("TypeID"),
                        L("TransType"),
                        L("AccountID"),
                        L("SubAccID"),
                        L("InvoiceNo"),
                        L("PartyInvNo"),
                        L("PartyInvDate"),
                        L("PartyInvAmount"),
                        L("Amount"),
                        L("Reason"),
                        L("Posted"),
                        L("LinkDetID"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, CRDRNote,
                        _ => _.CRDRNote.LocID,
                        _ => _.CRDRNote.DocNo,
                        _ => _timeZoneConverter.Convert(_.CRDRNote.DocDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.CRDRNote.PostingDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.CRDRNote.PaymentDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.CRDRNote.TypeID,
                        _ => _.CRDRNote.TransType,
                        _ => _.CRDRNote.AccountID,
                        _ => _.CRDRNote.SubAccID,
                        _ => _.CRDRNote.InvoiceNo,
                        _ => _.CRDRNote.PartyInvNo,
                        _ => _timeZoneConverter.Convert(_.CRDRNote.PartyInvDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.CRDRNote.PartyInvAmount,
                        _ => _.CRDRNote.Amount,
                        _ => _.CRDRNote.Reason,
                        _ => _.CRDRNote.Posted,
                        _ => _.CRDRNote.LinkDetID,
                        _ => _.CRDRNote.AudtUser,
                        _ => _timeZoneConverter.Convert(_.CRDRNote.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.CRDRNote.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.CRDRNote.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

                    var docDateColumn = sheet.Column(3);
                    docDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    docDateColumn.AutoFit();
                    var postingDateColumn = sheet.Column(4);
                    postingDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    postingDateColumn.AutoFit();
                    var paymentDate = sheet.Column(5);
                    paymentDate.Style.Numberformat.Format = "yyyy-mm-dd";
                    paymentDate.AutoFit();
                    var partyInvDate = sheet.Column(12);
                    partyInvDate.Style.Numberformat.Format = "yyyy-mm-dd";
                    partyInvDate.AutoFit();
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
