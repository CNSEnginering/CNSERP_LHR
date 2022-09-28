using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.AllowanceSetup.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.AllowanceSetup.Exporting
{
    public class AllowanceSetupExcelExporter : EpPlusExcelExporterBase, IAllowanceSetupExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AllowanceSetupExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAllowanceSetupForViewDto> allowanceSetup)
        {
            return CreateExcelPackage(
                "AllowanceSetup.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("AllowanceSetup"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DocID"),
                        L("FuelRate"),
                        L("MilageRate"),
                        L("RepairRate"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, allowanceSetup,
                        _ => _.AllowanceSetup.DocID,
                        _ => _.AllowanceSetup.FuelRate,
                        _ => _.AllowanceSetup.MilageRate,
                        _ => _.AllowanceSetup.RepairRate,
                        _ => _.AllowanceSetup.Active,
                        _ => _.AllowanceSetup.AudtUser,
                        _ => _timeZoneConverter.Convert(_.AllowanceSetup.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.AllowanceSetup.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.AllowanceSetup.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var audtDateColumn = sheet.Column(7);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
					var createDateColumn = sheet.Column(9);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createDateColumn.AutoFit();
					
					
                });
        }
    }
}
