using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.CommonServices.ChequeBooks.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.CommonServices.ChequeBooks.Exporting
{
    public class ChequeBooksExcelExporter : EpPlusExcelExporterBase, IChequeBooksExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ChequeBooksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetChequeBookForViewDto> chequeBooks)
        {
            return CreateExcelPackage(
                "ChequeBooks.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ChequeBooks"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DocNo"),
                        L("DocDate"),
                        L("BANKID"),
                        L("BankAccNo"),
                        L("FromChNo"),
                        L("ToChNo"),
                        L("NoofCh"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, chequeBooks,
                        _ => _.ChequeBook.DocNo,
                        _ => _timeZoneConverter.Convert(_.ChequeBook.DocDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ChequeBook.BANKID,
                        _ => _.ChequeBook.BankAccNo,
                        _ => _.ChequeBook.FromChNo,
                        _ => _.ChequeBook.ToChNo,
                        _ => _.ChequeBook.NoofCh,
                        _ => _.ChequeBook.Active,
                        _ => _.ChequeBook.AudtUser,
                        _ => _timeZoneConverter.Convert(_.ChequeBook.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ChequeBook.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.ChequeBook.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var docDateColumn = sheet.Column(2);
                    docDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					docDateColumn.AutoFit();
					var audtDateColumn = sheet.Column(10);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
					var createDateColumn = sheet.Column(12);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createDateColumn.AutoFit();
					
					
                });
        }
    }
}
