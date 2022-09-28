using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.SupplyChain.Sales.OECSH.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.SupplyChain.Sales.OECSH.Exporting
{
    public class OECSHExcelExporter : EpPlusExcelExporterBase, IOECSHExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OECSHExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOECSHForViewDto> oecsh)
        {
            return CreateExcelPackage(
                "OECSH.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("OECSH"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("LocID"),
                        L("DocNo"),
                        L("DocDate"),
                        L("SaleDoc"),
                        L("MDocNo"),
                        L("MDocDate"),
                        L("TypeID"),
                        L("SalesCtrlAcc"),
                        L("CustID"),
                        L("Narration"),
                        L("NoteText"),
                        L("PayTerms"),
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
                        L("BasicStyle"),
                        L("License"),
                        L("DirectCost"),
                        L("CommRate"),
                        L("CommAmt"),
                        L("OHRate"),
                        L("OHAmt"),
                        L("TaxRate"),
                        L("TaxAmt"),
                        L("TotalCost"),
                        L("ProfRate"),
                        L("ProfAmt"),
                        L("SalePrice"),
                        L("CostPP"),
                        L("SalePP"),
                        L("SaleUS")
                        );

                    AddObjects(
                        sheet, 2, oecsh,
                        _ => _.OECSH.LocID,
                        _ => _.OECSH.DocNo,
                        _ => _timeZoneConverter.Convert(_.OECSH.DocDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.OECSH.SaleDoc,
                        _ => _.OECSH.MDocNo,
                        _ => _timeZoneConverter.Convert(_.OECSH.MDocDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.OECSH.TypeID,
                        _ => _.OECSH.SalesCtrlAcc,
                        _ => _.OECSH.CustID,
                        _ => _.OECSH.Narration,
                        _ => _.OECSH.NoteText,
                        _ => _.OECSH.PayTerms,
                        _ => _.OECSH.DelvTerms,
                        _ => _.OECSH.ValidityTerms,
                        _ => _.OECSH.Approved,
                        _ => _.OECSH.ApprovedBy,
                        _ => _timeZoneConverter.Convert(_.OECSH.ApprovedDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.OECSH.Active,
                        _ => _.OECSH.AudtUser,
                        _ => _timeZoneConverter.Convert(_.OECSH.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.OECSH.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.OECSH.CreateDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.OECSH.BasicStyle,
                        _ => _.OECSH.License,
                        _ => _.OECSH.DirectCost,
                        _ => _.OECSH.CommRate,
                        _ => _.OECSH.CommAmt,
                        _ => _.OECSH.OHRate,
                        _ => _.OECSH.OHAmt,
                        _ => _.OECSH.TaxRate,
                        _ => _.OECSH.TaxAmt,
                        _ => _.OECSH.TotalCost,
                        _ => _.OECSH.ProfRate,
                        _ => _.OECSH.ProfAmt,
                        _ => _.OECSH.SalePrice,
                        _ => _.OECSH.CostPP,
                        _ => _.OECSH.SalePP,
                        _ => _.OECSH.SaleUS
                        );

                    var docDateColumn = sheet.Column(3);
                    docDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    docDateColumn.AutoFit();
                    var mDocDateColumn = sheet.Column(6);
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