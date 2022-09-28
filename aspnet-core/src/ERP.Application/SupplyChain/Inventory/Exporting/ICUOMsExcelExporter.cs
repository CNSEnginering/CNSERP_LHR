using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.SupplyChain.Inventory.Exporting
{
    public class ICUOMsExcelExporter : EpPlusExcelExporterBase, IICUOMsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ICUOMsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<ICUOMDto> icuoMs)
        {
            return CreateExcelPackage(
                "ICUOMs.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ICUOMs"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Unit"),
                        L("UNITDESC"),
                        L("Conver"),
                        L("Active"),
                        L("CreatedBy"),
                        L("CreateDate"),
                        L("AudtUser"),
                        L("AudtDate")
                        );

                    AddObjects(
                        sheet, 2, icuoMs,
                        _ => _.Unit,
                        _ => _.UNITDESC,
                        _ => _.Conver,
                        _ => _.Active,
                        _ => _.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.CreateDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.AudtUser,
                        _ => _timeZoneConverter.Convert(_.AudtDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var createDateColumn = sheet.Column(6);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createDateColumn.AutoFit();
					var audtDateColumn = sheet.Column(8);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
					

                });
        }
    }
}
