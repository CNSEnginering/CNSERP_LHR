using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.Transaction.BankReconcile.Dtos;
using ERP.Dto;
using ERP.Storage;
using ERP.GeneralLedger.Transaction.Dtos;

namespace ERP.GeneralLedger.Transaction.BankReconcile.Exporting
{
    public class GlChequesExcelExporter : EpPlusExcelExporterBase, IGlChequesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public GlChequesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetGlChequeForViewDto> glCheques)
        {
            return CreateExcelPackage(
                "GlCheques.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("GlCheques"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,                
                        L("DocNo"),
                        L("Type"),
                        L("EntryDate"),
                        L("ChequeDate"));

                    AddObjects(
                        sheet, 2, glCheques,
                       
                        _ => _.GlCheque.DocID,
                        _ => _.GlCheque.TypeID == 0 ? "Issued" : "Received",
                        _ => _.GlCheque.EntryDate,
                        _ => _.GlCheque.ChequeDate
                        );

					var docDateColumn = sheet.Column(3);
                    docDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					docDateColumn.AutoFit();
					var audtDateColumn = sheet.Column(4);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();				
                });
        }
    }
}
