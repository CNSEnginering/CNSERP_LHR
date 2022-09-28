using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.Transaction.BankReconcile.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.Transaction.BankReconcile.Exporting
{
    public class BankReconcileDetailsExcelExporter : EpPlusExcelExporterBase, IBankReconcileDetailsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BankReconcileDetailsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBankReconcileDetailForViewDto> bankReconcileDetails)
        {
            return CreateExcelPackage(
                "BankReconcileDetails.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("BankReconcileDetails"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DetID"),
                        L("BookID"),
                        L("ConfigID"),
                        L("VoucherID"),
                        L("VoucherDate"),
                        L("ClearingDate"),
                        L("Amount"),
                        L("Include")
                        );

                    AddObjects(
                        sheet, 2, bankReconcileDetails,
                        _ => _.BankReconcileDetail.DetID,
                        _ => _.BankReconcileDetail.BookID,
                        _ => _.BankReconcileDetail.ConfigID,
                        _ => _.BankReconcileDetail.VoucherID,
                        _ => _timeZoneConverter.Convert(_.BankReconcileDetail.VoucherDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.BankReconcileDetail.ClearingDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.BankReconcileDetail.Amount,
                        _ => _.BankReconcileDetail.Include
                        );

					var voucherDateColumn = sheet.Column(5);
                    voucherDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					voucherDateColumn.AutoFit();
					var clearingDateColumn = sheet.Column(6);
                    clearingDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					clearingDateColumn.AutoFit();
					

                });
        }
    }
}
