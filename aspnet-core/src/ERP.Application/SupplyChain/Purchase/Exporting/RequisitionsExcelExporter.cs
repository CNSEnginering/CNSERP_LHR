using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.Storage;
using ERP.SupplyChain.Purchase.Requisition.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Purchase.Exporting
{
    public class RequisitionsExcelExporter : EpPlusExcelExporterBase, IRequisitionsExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RequisitionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRequisitionForViewDto> requisition)
        {
            return CreateExcelPackage(
                "Requisition.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Requisition"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("locID"),
                        L("locName"),
                        L("DocNo"),
                        L("DocDate")
                        );
                    AddObjects(
                        sheet, 2, requisition,
                        _ => _.Requisition.LocID,
                        _ => _.Requisition.LocName,
                        _ => _.Requisition.DocNo,
                        _ => _.Requisition.DocDate.Value.Date
                        );
                });
        }
    }
}
