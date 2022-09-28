using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.SupplyChain.Sales.SaleQutation.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.SupplyChain.Sales.SaleQutation.Exporting
{
    public class OEQHExcelExporter : EpPlusExcelExporterBase, IOEQHExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OEQHExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOEQHForViewDto> oeqh)
        {
            return CreateExcelPackage(
                "OEQH.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("OEQH"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("LocID"),
                        L("DocNo"),
                        L("DocDate"),
                        L("MDocNo"),
                        L("MDocDate"),
                        L("TypeID"),
                        L("SalesCtrlAcc"),
                        L("CustID"),
                        L("Narration"),
                        L("NoteText"),
                        L("PayTerms"),
                        L("NetAmount"),
                        L("DelvTerms"),
                        L("ValidityTerms"),
                        L("Approved"),
                        L("ApprovedBy"),
                        L("ApprovedDate"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate"),
                        L("TaxAuth1"),
                        L("TaxClass1"),
                        L("TaxClassDesc1"),
                        L("TaxRate1"),
                        L("TaxAmt1"),
                        L("TaxAuth2"),
                        L("TaxClassDesc2"),
                        L("TaxClass2"),
                        L("TaxRate2"),
                        L("TaxAmt2"),
                        L("TaxAuth3"),
                        L("TaxClassDesc3"),
                        L("TaxClass3"),
                        L("TaxRate3"),
                        L("TaxAmt3"),
                        L("TaxAuth4"),
                        L("TaxClassDesc4"),
                        L("TaxClass4"),
                        L("TaxRate4"),
                        L("TaxAmt4"),
                        L("TaxAuth5"),
                        L("TaxClassDesc5"),
                        L("TaxClass5"),
                        L("TaxRate5"),
                        L("TaxAmt5")
                        );

                    AddObjects(
                        sheet, 2, oeqh,
                        _ => _.OEQH.LocID,
                        _ => _.OEQH.DocNo,
                        _ => _timeZoneConverter.Convert(_.OEQH.DocDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.OEQH.MDocNo,
                        _ => _timeZoneConverter.Convert(_.OEQH.MDocDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.OEQH.TypeID,
                        _ => _.OEQH.SalesCtrlAcc,
                        _ => _.OEQH.CustID,
                        _ => _.OEQH.Narration,
                        _ => _.OEQH.NoteText,
                        _ => _.OEQH.PayTerms,
                        _ => _.OEQH.NetAmount,
                        _ => _.OEQH.DelvTerms,
                        _ => _.OEQH.ValidityTerms,
                        _ => _.OEQH.Approved,
                        _ => _.OEQH.ApprovedBy,
                        _ => _timeZoneConverter.Convert(_.OEQH.ApprovedDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.OEQH.Active,
                        _ => _.OEQH.AudtUser,
                        _ => _timeZoneConverter.Convert(_.OEQH.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.OEQH.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.OEQH.CreateDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.OEQH.TaxAuth1,
                        _ => _.OEQH.TaxClass1,
                        _ => _.OEQH.TaxClassDesc1,
                        _ => _.OEQH.TaxRate1,
                        _ => _.OEQH.TaxAmt1,
                        _ => _.OEQH.TaxAuth2,
                        _ => _.OEQH.TaxClassDesc2,
                        _ => _.OEQH.TaxClass2,
                        _ => _.OEQH.TaxRate2,
                        _ => _.OEQH.TaxAmt2,
                        _ => _.OEQH.TaxAuth3,
                        _ => _.OEQH.TaxClassDesc3,
                        _ => _.OEQH.TaxClass3,
                        _ => _.OEQH.TaxRate3,
                        _ => _.OEQH.TaxAmt3,
                        _ => _.OEQH.TaxAuth4,
                        _ => _.OEQH.TaxClassDesc4,
                        _ => _.OEQH.TaxClass4,
                        _ => _.OEQH.TaxRate4,
                        _ => _.OEQH.TaxAmt4,
                        _ => _.OEQH.TaxAuth5,
                        _ => _.OEQH.TaxClassDesc5,
                        _ => _.OEQH.TaxClass5,
                        _ => _.OEQH.TaxRate5,
                        _ => _.OEQH.TaxAmt5
                        );

                    var docDateColumn = sheet.Column(3);
                    docDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    docDateColumn.AutoFit();
                    var mDocDateColumn = sheet.Column(5);
                    mDocDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    mDocDateColumn.AutoFit();
                    var approvedDateColumn = sheet.Column(17);
                    approvedDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    approvedDateColumn.AutoFit();
                    var audtDateColumn = sheet.Column(20);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    audtDateColumn.AutoFit();
                    var createDateColumn = sheet.Column(22);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    createDateColumn.AutoFit();

                });
        }
    }
}