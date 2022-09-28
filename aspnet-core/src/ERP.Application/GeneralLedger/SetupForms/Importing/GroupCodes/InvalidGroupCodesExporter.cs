using System.Collections.Generic;
using Abp.Collections.Extensions;
using ERP.Authorization.Users.Importing.Dto;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.GeneralLedger.SetupForms.Importing.GroupCodes.Dto;
using ERP.GeneralLedger.SetupForms.Importing.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.Importing.GroupCodes
{
    public class InvalidGroupCodesExporter : EpPlusExcelExporterBase, IInvalidGroupCodesExporter
    {
        public InvalidGroupCodesExporter(ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<ImportGroupCodesDto> GroupCodesListDtos)
        {
            return CreateExcelPackage(
                "InvalidGroupCodesImportList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("InvalidSegment1Imports"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("AccountID"),
                        L("SubAccID"),
                        L("SubAccName"),
                        L("Address1"),
                        L("Address2"),
                        L("City")
                       
                        );

                    AddObjects(
                        sheet, 2, GroupCodesListDtos,
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
