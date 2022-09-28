using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.CommonServices.Dtos;
using ERP.Dto;
using ERP.Storage;
using ERP.SupplyChain.Inventory.Dtos;

namespace ERP.SupplyChain.Inventory.Exporting
{
    public class AssemblyExcelExporter : EpPlusExcelExporterBase, IAssemblyExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AssemblyExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAssemblyForViewDto> assembly)
        {
            return CreateExcelPackage(
                "Assembly.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Assembly"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DocNo"),
                        L("DocDate"),
                        L("Loc")
                        );

                    AddObjects(
                        sheet, 2, assembly,
                        _ => _.Assembly.DocNo,
                        _ => _.Assembly.DocDate,
                        _ => _.Assembly.locName
                        );
                });
        }
    }
}
