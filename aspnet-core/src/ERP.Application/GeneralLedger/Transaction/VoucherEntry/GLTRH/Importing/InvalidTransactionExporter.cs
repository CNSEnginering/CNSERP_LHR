using System.Collections.Generic;
using Abp.Collections.Extensions;
using ERP.Authorization.Users.Importing.Dto;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Importing
{
    public class InvalidTransactionExporter : EpPlusExcelExporterBase, IInvalidTransactionExporter
    {
        public InvalidTransactionExporter(ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<ImportTransactionDto> TransactionListDtos)
        {
            return CreateExcelPackage(
                "InvalidTransactionImportList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("InvalidSegment1Imports"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Seg1ID"),
                        L("SegmentName"),
                        L("Family"),
                        L("OldCode")
                       
                        );

                    AddObjects(
                        sheet, 2, TransactionListDtos,
                        _ => _.Id,
                        _ => _.NARRATION,
                        _ => _.Posted,
                        _ => _.OldCode
                        );

                    for (var i = 1; i <= 4; i++)
                    {
                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
