using System.Collections.Generic;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.GeneralLedger.SetupForms.Importing.SegmentLevel3.Dto;
using ERP.GeneralLedger.SetupForms.Importing.SegmentLevel3;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.Importing.SegmentLevel3
{
    public class InvalidSegmentLevel3Exporter : EpPlusExcelExporterBase, IInvalidSegmentLevel3Exporter
    {
        public InvalidSegmentLevel3Exporter(ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<ImportSegmentLevel3Dto> controlDetailListDtos)
        {
            return CreateExcelPackage(
                "InvalidSegmentLevel3ImportList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("InvalidSegment3Imports"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                       L("Seg3ID"),
                        L("SegmentName3"),
                        L("OldCode")

                        );

                    AddObjects(
                        sheet, 2, controlDetailListDtos,
                        _ => _.Seg3ID,
                        _ => _.SegmentName,
                        _ => _.OldCode
                        );

                    for (var i = 1; i <= 3; i++)
                    {
                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
