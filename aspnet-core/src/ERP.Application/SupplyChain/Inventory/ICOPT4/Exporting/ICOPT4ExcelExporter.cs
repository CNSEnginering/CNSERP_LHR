using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.SupplyChain.Inventory.ICOPT4.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.SupplyChain.Inventory.ICOPT4.Exporting
{
    public class ICOPT4ExcelExporter : EpPlusExcelExporterBase, IICOPT4ExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ICOPT4ExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetICOPT4ForViewDto> icopT4)
        {
            return CreateExcelPackage(
                "ICOPT4.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ICOPT4"));
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
                        sheet, 2, icopT4,
                        _ => _.ICOPT4.OptID,
                        _ => _.ICOPT4.Descp,
                        _ => _.ICOPT4.Active,
                        _ => _.ICOPT4.AudtUser,
                        _ => _timeZoneConverter.Convert(_.ICOPT4.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ICOPT4.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.ICOPT4.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
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
