using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.SupplyChain.Inventory.Adjustment.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.SupplyChain.Inventory.Adjustment.Exporting
{
    public class ICADJHeaderExcelExporter : EpPlusExcelExporterBase, IICADJHeaderExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ICADJHeaderExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetICADJHeaderForViewDto> adjustments)
        {
            return CreateExcelPackage(
                "Adjustments.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Adjustments"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DocNo"),
                        L("DocDate"),
                        L("Narration"),
                        L("LocID"),
                        L("TotalQty"),
                        L("TotalAmt"),
                        L("Posted"),
                        L("LinkDetID"),
                        L("OrdNo"),
                        L("Active"),
                        L("CreatedBy"),
                        L("CreateDate"),
                        L("AudtUser"),
                        L("AudtDate")
                        );

                    AddObjects(
                        sheet, 2, adjustments,
                        _ => _.Adjustment.DocNo,
                        _ => _timeZoneConverter.Convert(_.Adjustment.DocDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Adjustment.Narration,
                        _ => _.Adjustment.LocID,
                        _ => _.Adjustment.TotalQty,
                        _ => _.Adjustment.TotalAmt,
                        _ => _.Adjustment.Posted,
                        _ => _.Adjustment.LinkDetID,
                        _ => _.Adjustment.OrdNo,
                        _ => _.Adjustment.Active,
                        _ => _.Adjustment.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.Adjustment.CreateDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Adjustment.AudtUser,
                        _ => _timeZoneConverter.Convert(_.Adjustment.AudtDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var docDateColumn = sheet.Column(2);
                    docDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					docDateColumn.AutoFit();
					var createDateColumn = sheet.Column(12);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createDateColumn.AutoFit();
					var audtDateColumn = sheet.Column(14);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
					

                });
        }
    }
}
