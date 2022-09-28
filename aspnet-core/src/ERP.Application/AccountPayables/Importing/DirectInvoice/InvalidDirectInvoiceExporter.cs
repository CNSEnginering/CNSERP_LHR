using System.Collections.Generic;
using ERP.AccountPayables.Importing.DirectInvoice;
using ERP.AccountPayables.Importing.DirectInvoice.Dto;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;

using ERP.Storage;

namespace ERP.AccountPayables.Importing.ApTerms
{
    public class InvalidDirectInvoiceExporter : EpPlusExcelExporterBase, IInvalidDirectInvoiceExporter
    {
        public InvalidDirectInvoiceExporter(ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<ImportDirectInvoiceDto> DirectInvoiceListDtos)
        {
            return CreateExcelPackage(
                "InvalidDirectInvoiceImportList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("InvalidDirectInvoiceImports"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DocNo"),
                        L("CPRID"),
                        L("CPRNO"),
                        L("PaymentDate")
                        );

                    AddObjects(
                        sheet, 2, DirectInvoiceListDtos,
                        _ => _.DocNo,
                        _ => _.CprID,
                        _ => _.CprNo,
                        _ => _.CprDate
                        );

                    for (var i = 1; i <= 4; i++)
                    {
                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
