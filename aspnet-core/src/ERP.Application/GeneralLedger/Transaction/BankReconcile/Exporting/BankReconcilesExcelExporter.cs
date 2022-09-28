using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.Transaction.BankReconcile.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.Transaction.BankReconcile.Exporting
{
    public class BankReconcilesExcelExporter : EpPlusExcelExporterBase, IBankReconcilesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BankReconcilesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBankReconcileForViewDto> bankReconciles)
        {
            return CreateExcelPackage(
                "BankReconciles.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("BankReconciles"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                       
                        L("DocID"),
                        L("DocDate"),
                        L("BankID"),
                        L("BankName"),
                        L("BeginBalance"),
                        L("EndBalance"),
                        L("ReconcileAmt"),
                        L("DiffAmount"),
                        L("Narration"),
                        L("Completed"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreatedDate")
                        );

                    AddObjects(
                        sheet, 2, bankReconciles,
                       
                        _ => _.BankReconcile.DocID,
                        _ => _timeZoneConverter.Convert(_.BankReconcile.DocDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.BankReconcile.BankID,
                        _ => _.BankReconcile.BankName,
                        _ => _.BankReconcile.BeginBalance,
                        _ => _.BankReconcile.EndBalance,
                        _ => _.BankReconcile.ReconcileAmt,
                        _ => _.BankReconcile.DiffAmount,
                        _ => _.BankReconcile.Narration,
                        _ => _.BankReconcile.Completed,
                        _ => _.BankReconcile.AudtUser,
                        _ => _timeZoneConverter.Convert(_.BankReconcile.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.BankReconcile.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.BankReconcile.CreatedDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var docDateColumn = sheet.Column(3);
                    docDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					docDateColumn.AutoFit();
					var audtDateColumn = sheet.Column(13);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
					var createdDateColumn = sheet.Column(15);
                    createdDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createdDateColumn.AutoFit();
					

                });
        }
    }
}
