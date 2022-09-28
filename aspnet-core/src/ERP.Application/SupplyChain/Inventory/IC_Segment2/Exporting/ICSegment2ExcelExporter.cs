using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.Storage;
using ERP.SupplyChain.Inventory.IC_Segment2.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment2.Exporting
{
    public class ICSegment2ExcelExporter : EpPlusExcelExporterBase, IICSegment2ExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ICSegment2ExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetICSegment2ForViewDto> icSegment2)
        {
            return CreateExcelPackage(
                "ICSegment2.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ICSegment2"));

                    AddHeader(
                        sheet,
                       L("Segment1 ID"),
                        L("Seg1Name"),
                       L("Segment2 ID"),
                        L("Seg2Name")
                        
                        
                        );

                    AddObjects(
                        sheet, 2, icSegment2,
                         _ => _.ICSegment2.Seg1Id,
                         _ => _.ICSegment2.Seg1Name,
                         _ => _.ICSegment2.Seg2Id,
                        _ => _.ICSegment2.Seg2Name
                        
                       
                        );

					

                });
        }
    }
}
