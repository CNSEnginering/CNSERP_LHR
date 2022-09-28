using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.SupplyChain.Purchase.APINVH.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.SupplyChain.Purchase.APINVH.Exporting
{
    public class APINVHExcelExporter : EpPlusExcelExporterBase, IAPINVHExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public APINVHExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAPINVHForViewDto> apinvh)
        {
            return CreateExcelPackage(
                "APINVH.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("APINVH"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DocNo"),
                        L("VAccountID"),
                        L("SubAccID"),
                        L("PartyInvNo"),
                        L("PartyInvDate"),
                        L("Amount"),
                        L("DiscAmount"),
                        L("PaymentOption"),
                        L("BankID"),
                        L("BAccountID"),
                        L("ConfigID"),
                        L("ChequeNo"),
                        L("ChType"),
                        L("CurID"),
                        L("CurRate"),
                        L("TaxAuth"),
                        L("TaxClass"),
                        L("TaxRate"),
                        L("TaxAccID"),
                        L("TaxAmount"),
                        L("DocDate"),
                        L("PostDate"),
                        L("Narration"),
                        L("RefNo"),
                        L("PayReason"),
                        L("Posted"),
                        L("PostedBy"),
                        L("PostedDate"),
                        L("LinkDetID"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate"),
                        L("DocStatus"),
                        L("CprID"),
                        L("CprNo"),
                        L("CprDate")
                        );

                    AddObjects(
                        sheet, 2, apinvh,
                        _ => _.APINVH.DocNo,
                        _ => _.APINVH.VAccountID,
                        _ => _.APINVH.SubAccID,
                        _ => _.APINVH.PartyInvNo,
                        _ => _timeZoneConverter.Convert(_.APINVH.PartyInvDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.APINVH.Amount,
                        _ => _.APINVH.DiscAmount,
                        _ => _.APINVH.PaymentOption,
                        _ => _.APINVH.BankID,
                        _ => _.APINVH.BAccountID,
                        _ => _.APINVH.ConfigID,
                        _ => _.APINVH.ChequeNo,
                        _ => _.APINVH.ChType,
                        _ => _.APINVH.CurID,
                        _ => _.APINVH.CurRate,
                        _ => _.APINVH.TaxAuth,
                        _ => _.APINVH.TaxClass,
                        _ => _.APINVH.TaxRate,
                        _ => _.APINVH.TaxAccID,
                        _ => _.APINVH.TaxAmount,
                        _ => _timeZoneConverter.Convert(_.APINVH.DocDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.APINVH.PostDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.APINVH.Narration,
                        _ => _.APINVH.RefNo,
                        _ => _.APINVH.PayReason,
                        _ => _.APINVH.Posted,
                        _ => _.APINVH.PostedBy,
                        _ => _timeZoneConverter.Convert(_.APINVH.PostedDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.APINVH.LinkDetID,
                        _ => _.APINVH.AudtUser,
                        _ => _timeZoneConverter.Convert(_.APINVH.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.APINVH.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.APINVH.CreateDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.APINVH.DocStatus,
                        _ => _.APINVH.CprID,
                        _ => _.APINVH.CprNo,
                        _ => _timeZoneConverter.Convert(_.APINVH.CprDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

                    var partyInvDateColumn = sheet.Column(5);
                    partyInvDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    partyInvDateColumn.AutoFit();
                    var docDateColumn = sheet.Column(21);
                    docDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    docDateColumn.AutoFit();
                    var postDateColumn = sheet.Column(22);
                    postDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    postDateColumn.AutoFit();
                    var postedDateColumn = sheet.Column(28);
                    postedDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    postedDateColumn.AutoFit();
                    var audtDateColumn = sheet.Column(31);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    audtDateColumn.AutoFit();
                    var createDateColumn = sheet.Column(33);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    createDateColumn.AutoFit();
                    var cprDateColumn = sheet.Column(37);
                    cprDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    cprDateColumn.AutoFit();

                });
        }
    }
}