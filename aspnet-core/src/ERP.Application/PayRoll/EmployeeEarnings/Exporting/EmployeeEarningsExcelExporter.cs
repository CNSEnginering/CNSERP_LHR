using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.EmployeeEarnings.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.EmployeeEarnings.Exporting
{
    public class EmployeeEarningsExcelExporter : EpPlusExcelExporterBase, IEmployeeEarningsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeeEarningsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeeEarningsForViewDto> employeeEarnings)
        {
            return CreateExcelPackage(
                "EmployeeEarnings.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("EmployeeEarnings"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("EarningID"),
                        L("EmployeeID"),
                        L("SalaryYear"),
                        L("SalaryMonth"),
                        L("EarningDate"),
                        L("Amount"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, employeeEarnings,
                        _ => _.EmployeeEarnings.EarningID,
                        _ => _.EmployeeEarnings.EmployeeID,
                        _ => _.EmployeeEarnings.SalaryYear,
                        _ => _.EmployeeEarnings.SalaryMonth,
                        _ => _timeZoneConverter.Convert(_.EmployeeEarnings.EarningDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.EmployeeEarnings.Amount,
                        _ => _.EmployeeEarnings.Active,
                        _ => _.EmployeeEarnings.AudtUser,
                        _ => _timeZoneConverter.Convert(_.EmployeeEarnings.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.EmployeeEarnings.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.EmployeeEarnings.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var earningDateColumn = sheet.Column(5);
                    earningDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					earningDateColumn.AutoFit();
					var audtDateColumn = sheet.Column(9);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
					var createDateColumn = sheet.Column(11);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createDateColumn.AutoFit();
					

                });
        }
    }
}
