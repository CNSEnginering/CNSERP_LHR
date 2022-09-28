using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.SupplyChain.Inventory.Exporting
{
    public class ReorderLevelsExcelExporter : EpPlusExcelExporterBase, IReorderLevelsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ReorderLevelsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<ReorderLevelDto> reorderLevels)
        {
            return CreateExcelPackage(
                "ReorderLevels.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ReorderLevels"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("MinLevel"),
                        L("MaxLevel"),
                        L("OrdLevel"),
                        L("CreatedBy"),
                        L("CreateDate"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("LocId"),
                        L("ItemId")
                        );

                    AddObjects(
                        sheet, 2, reorderLevels,
                        _ => _.MinLevel,
                        _ => _.MaxLevel,
                        _ => _.OrdLevel,
                        _ => _.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.CreateDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.AudtUser,
                        _ => _timeZoneConverter.Convert(_.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.LocId,
                        _ => _.ItemId
                        );

					var createDateColumn = sheet.Column(5);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createDateColumn.AutoFit();
					var audtDateColumn = sheet.Column(7);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
					

                });
        }
    }
}
