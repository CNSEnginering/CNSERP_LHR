using System.Collections.Generic;
using Abp.Collections.Extensions;
using ERP.Authorization.Users.Importing.Dto;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.GeneralLedger.SetupForms.Importing.Dto;
using ERP.Storage;

namespace ERP.Authorization.Users.Importing.SubLedger
{
    public class InvalidSubLedgerExporter : EpPlusExcelExporterBase, IInvalidSubLedgerExporter
    {
        public InvalidSubLedgerExporter(ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<ImportSubLedgerDto> subLedgerListDtos)
        {
            return CreateExcelPackage(
                "InvalidSubledgerImportList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("InvalidSubLedgerImports"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("AccountID"),
                        L("SubAccID"),
                        L("SubAccName"),
                        L("Address1"),
                        L("Address2"),
                        L("City"),
                        L("Phone"),
                        L("Contact"),
                        L("RegNo"),
                        L("TAXAUTH"),
                        L("ClassID"),
                        L("SLType"),
                        L("LegalName"),
                        L("Country"),
                        L("STATE"),
                        L("CITY"),
                        L("Linked"),
                        L("ParentID"),
                        L("Refuse Reason")
                        );

                    AddObjects(
                        sheet, 2, subLedgerListDtos,
                        _ => _.AccountID,
                        _ => _.Id,
                        _ => _.SubAccName,
                        _ => _.Address1,
                        _ => _.Address2,
                        _ => _.City,
                        _ => _.Phone,
                        _ => _.Contact,
                        _ => _.RegNo,
                        _ => _.TAXAUTH,
                        _ => _.ClassID,
                        _ => _.SLType,
                        _ => _.LegalName,
                        _ => _.CountryID,
                        _ => _.ProvinceID,
                        _ => _.CityID,
                        _ => _.Linked,
                        _ => _.ParentID,
                        _ => _.Exception
                        );

                    for (var i = 1; i <= 9; i++)
                    {
                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
