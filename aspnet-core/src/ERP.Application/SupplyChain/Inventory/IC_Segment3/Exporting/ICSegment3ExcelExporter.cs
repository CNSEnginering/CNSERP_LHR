using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.Storage;
using ERP.SupplyChain.Inventory.IC_Segment3.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment3.Exporting
{
    public class ICSegment3ExcelExporter : EpPlusExcelExporterBase, IICSegment3ExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ICSegment3ExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetICSegment3ForViewDto> icSegment3)
        {
            return CreateExcelPackage(
                "ICSegment3.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ICSegment3"));

                    AddHeader(
                        sheet,
                        L("Segment1 ID"),
                        L("Seg1Name"),
                         L("Segment2 ID"),
                        L("Seg2Name"),
                         L("Segment3 ID"),
                        L("Seg3Name")
                        );

                    AddObjects(
                        sheet, 2, icSegment3,
                        _ => _.ICSegment3.Seg1Id,
                        _ => _.Seg1Name,
                        _ => _.ICSegment3.Seg2Id,
                        _ => _.Seg2Name,
                         _ => _.ICSegment3.Seg3Id,
                        _ => _.ICSegment3.Seg3Name
                        );

					

                });
        }
    }
}
