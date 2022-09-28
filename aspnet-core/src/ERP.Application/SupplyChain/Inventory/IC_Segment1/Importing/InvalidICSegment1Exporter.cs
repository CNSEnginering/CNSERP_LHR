using System.Collections.Generic;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.Storage;
using ERP.SupplyChain.Inventory.IC_Segment1.Importing.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment1.Importing
{
    public class InvalidICSegment1Exporter : EpPlusExcelExporterBase, IInvalidICSegment1Exporter
    {
        public InvalidICSegment1Exporter(ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<ImportICSegment1Dto> ICSegment1ListDtos)
        {
            return CreateExcelPackage(
                "InvalidICSegment1ImportList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("InvalidSegment1Imports"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Seg1ID"),
                        L("Seg1Name")

                        );

                    AddObjects(
                        sheet, 2, ICSegment1ListDtos,
                        _ => _.Seg1ID,
                        _ => _.Seg1Name
                       
                        );

                    for (var i = 1; i <= 2; i++)
                    {
                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
