using System.Collections.Generic;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.Storage;
using ERP.SupplyChain.Inventory.IC_Segment2.Importing.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment2.Importing
{
    public class InvalidICSegment2Exporter : EpPlusExcelExporterBase, IInvalidICSegment2Exporter
    {
        public InvalidICSegment2Exporter(ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<ImportICSegment2Dto> ICSegment2ListDtos)
        {
            return CreateExcelPackage(
                "InvalidICSegment2ImportList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("InvalidSegment2Imports"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Segment 1"),
                        L("Segment 2"),
                        L("Seg2Name")

                        );

                    AddObjects(
                        sheet, 2, ICSegment2ListDtos,
                         _ => _.Seg1Id,
                         _ => _.Seg2Id,
                        _ => _.Seg2Name

                        );

                    for (var i = 1; i <= 2; i++)
                    {
                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
