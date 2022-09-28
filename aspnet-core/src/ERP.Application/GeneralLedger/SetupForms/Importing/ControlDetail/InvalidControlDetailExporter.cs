using System.Collections.Generic;
using Abp.Collections.Extensions;
using ERP.Authorization.Users.Importing.Dto;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.GeneralLedger.SetupForms.Importing.ControlDetail.Dto;
using ERP.GeneralLedger.SetupForms.Importing.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.Importing.ControlDetail
{
    public class InvalidControlDetailExporter : EpPlusExcelExporterBase, IInvalidControlDetailExporter
    {
        public InvalidControlDetailExporter(ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<ImportControlDetailDto> controlDetailListDtos)
        {
            return CreateExcelPackage(
                "InvalidControlDetailImportList.xlsx",
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
                        sheet, 2, controlDetailListDtos,
                        _ => _.Seg1ID,
                        _ => _.SegmentName,
                        _ => _.Family,
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
