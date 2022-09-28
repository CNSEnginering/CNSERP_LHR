using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.CommonServices.RecurringVoucher.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.CommonServices.RecurringVoucher.Exporting
{
    public class RecurringVouchersExcelExporter : EpPlusExcelExporterBase, IRecurringVouchersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RecurringVouchersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRecurringVoucherForViewDto> recurringVouchers)
        {
            return CreateExcelPackage(
                "RecurringVouchers.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("RecurringVouchers"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DocNo"),
                        L("BookID"),
                        L("VoucherNo"),
                        L("FmtVoucherNo"),
                        L("VoucherDate"),
                        L("VoucherMonth"),
                        L("ConfigID"),
                        L("Reference"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, recurringVouchers,
                        _ => _.RecurringVoucher.DocNo,
                        _ => _.RecurringVoucher.BookID,
                        _ => _.RecurringVoucher.VoucherNo,
                        _ => _.RecurringVoucher.FmtVoucherNo,
                        _ => _timeZoneConverter.Convert(_.RecurringVoucher.VoucherDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.RecurringVoucher.VoucherMonth,
                        _ => _.RecurringVoucher.ConfigID,
                        _ => _.RecurringVoucher.Reference,
                        _ => _.RecurringVoucher.Active,
                        _ => _.RecurringVoucher.AudtUser,
                        _ => _timeZoneConverter.Convert(_.RecurringVoucher.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.RecurringVoucher.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.RecurringVoucher.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var voucherDateColumn = sheet.Column(5);
                    voucherDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					voucherDateColumn.AutoFit();
					var audtDateColumn = sheet.Column(11);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
					var createDateColumn = sheet.Column(13);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createDateColumn.AutoFit();
					
					
                });
        }
    }
}
