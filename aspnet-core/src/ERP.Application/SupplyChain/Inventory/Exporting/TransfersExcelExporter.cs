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
    public class TransfersExcelExporter : EpPlusExcelExporterBase, ITransfersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TransfersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTransfersForViewDto> transfers)
        {
            return CreateExcelPackage(
                "Transfers.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Transfers"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DocNo"),
                        L("DocDate"),
                        L("FromLoc"),
                        L("ToLoc")
                        );
                    AddObjects(
                        sheet, 2, transfers,
                        _ => _.Transfer.DocNo,
                        _ => _.Transfer.DocDate,
                        _ => _.Transfer.FromLocName,
                        _ => _.Transfer.ToLocName
                        );
                });
        }
    }
}
