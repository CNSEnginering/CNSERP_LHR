using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.CommonServices.ChequeBooks.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.CommonServices.ChequeBooks.Exporting
{
    public class ChequeBookDetailsExcelExporter : EpPlusExcelExporterBase, IChequeBookDetailsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ChequeBookDetailsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetChequeBookDetailForViewDto> chequeBookDetails)
        {
            return CreateExcelPackage(
                "ChequeBookDetails.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ChequeBookDetails"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DetID"),
                        L("DocNo"),
                        L("BANKID"),
                        L("BankAccNo"),
                        L("FromChNo"),
                        L("ToChNo"),
                        L("BooKID"),
                        L("VoucherNo"),
                        L("VoucherDate"),
                        L("Narration"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, chequeBookDetails,
                        _ => _.ChequeBookDetail.DetID,
                        _ => _.ChequeBookDetail.DocNo,
                        _ => _.ChequeBookDetail.BANKID,
                        _ => _.ChequeBookDetail.BankAccNo,
                        _ => _.ChequeBookDetail.FromChNo,
                        _ => _.ChequeBookDetail.ToChNo,
                        _ => _.ChequeBookDetail.BooKID,
                        _ => _.ChequeBookDetail.VoucherNo,
                        _ => _timeZoneConverter.Convert(_.ChequeBookDetail.VoucherDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ChequeBookDetail.Narration,
                        _ => _.ChequeBookDetail.AudtUser,
                        _ => _timeZoneConverter.Convert(_.ChequeBookDetail.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ChequeBookDetail.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.ChequeBookDetail.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var voucherDateColumn = sheet.Column(9);
                    voucherDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					voucherDateColumn.AutoFit();
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
