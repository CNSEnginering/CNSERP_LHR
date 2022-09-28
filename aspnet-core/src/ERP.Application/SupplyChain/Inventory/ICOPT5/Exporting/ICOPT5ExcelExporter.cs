using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.SupplyChain.Inventory.ICOPT5.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.SupplyChain.Inventory.ICOPT5.Exporting
{
    public class ICOPT5ExcelExporter : EpPlusExcelExporterBase, IICOPT5ExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ICOPT5ExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetICOPT5ForViewDto> icopT5)
        {
            return CreateExcelPackage(
                "ICOPT5.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ICOPT5"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("OptID"),
                        L("Descp"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, icopT5,
                        _ => _.ICOPT5.OptID,
                        _ => _.ICOPT5.Descp,
                        _ => _.ICOPT5.Active,
                        _ => _.ICOPT5.AudtUser,
                        _ => _timeZoneConverter.Convert(_.ICOPT5.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ICOPT5.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.ICOPT5.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var audtDateColumn = sheet.Column(5);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
					var createDateColumn = sheet.Column(7);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createDateColumn.AutoFit();
					

                });
        }
    }
}
