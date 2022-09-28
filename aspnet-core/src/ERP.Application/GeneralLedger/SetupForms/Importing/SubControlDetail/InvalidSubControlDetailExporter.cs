using System.Collections.Generic;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.GeneralLedger.SetupForms.Importing.SubControlDetail.Dto;
using ERP.GeneralLedger.SetupForms.Importing.SUbControlDetail;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.Importing.SubControlDetail
{
    public class InvalidSubControlDetailExporter : EpPlusExcelExporterBase, IInvalidSubControlDetailExporter
    {
        public InvalidSubControlDetailExporter(ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<ImportSubControlDetailDto> controlDetailListDtos)
        {
            return CreateExcelPackage(
                "InvalidSubControlDetailImportList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("InvalidSegment2Imports"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                       L("Seg2ID"),
                        L("SegmentName2"),
                        L("OldCode")

                        );

                    AddObjects(
                        sheet, 2, controlDetailListDtos,
                        _ => _.Seg2ID,
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
