using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.SupplyChain.Inventory.Exporting
{
    public class ICLocationsExcelExporter : EpPlusExcelExporterBase, IICLocationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ICLocationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<ICLocationDto> icLocations)
        {
            return CreateExcelPackage(
                "ICLocations.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ICLocations"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("LocID"),
                        L("LocName"),
                        L("LocShort"),
                        L("Address"),
                        L("City"),
                        L("AllowRec"),
                        L("AllowNeg"),
                        L("Active"),
                        L("CreatedBy"),
                        L("CreateDate"),
                        L("AudtUser"),
                        L("AudtDate")
                        );

                    AddObjects(
                        sheet, 2, icLocations,
                        _ => _.LocName,
                        _ => _.LocShort,
                        _ => _.Address,
                        _ => _.City,
                        _ => _.AllowRec,
                        _ => _.AllowNeg,
                        _ => _.Active,
                        _ => _.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.CreateDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.AudtUser,
                        _ => _timeZoneConverter.Convert(_.AudtDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var createDateColumn = sheet.Column(10);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createDateColumn.AutoFit();
					var audtDateColumn = sheet.Column(12);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
					

                });
        }
    }
}
