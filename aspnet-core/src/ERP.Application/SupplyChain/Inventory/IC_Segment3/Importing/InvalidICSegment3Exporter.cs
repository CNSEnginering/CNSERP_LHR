using System.Collections.Generic;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.Storage;
using ERP.SupplyChain.Inventory.IC_Segment3.Importing.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment3.Importing
{
    public class InvalidICSegment3Exporter : EpPlusExcelExporterBase, IInvalidICSegment3Exporter
    {
        public InvalidICSegment3Exporter(ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<ImportICSegment3Dto> ICSegment3ListDtos)
        {
            return CreateExcelPackage(
                "InvalidICSegment3ImportList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("InvalidSegment3Imports"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Segment1 ID"),
                        L("Segment2 ID"),
                        L("Segment3 ID"),
                        L("Seg3Name")

                        );

                    AddObjects(
                        sheet, 2, ICSegment3ListDtos,
                        _ => _.Seg1Id,
                        _ => _.Seg2Id,
                        _ => _.Seg3Id,
                        _ => _.Seg3Name

                        );

                    for (var i = 1; i <= 2; i++)
                    {
                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
