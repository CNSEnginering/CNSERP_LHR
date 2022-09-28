using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.EmployeeDeductions.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.EmployeeDeductions.Exporting
{
    public class EmployeeDeductionsExcelExporter : EpPlusExcelExporterBase, IEmployeeDeductionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeeDeductionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeeDeductionsForViewDto> employeeDeductions)
        {
            return CreateExcelPackage(
                "EmployeeDeductions.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("EmployeeDeductions"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DeductionID"),
                        L("EmployeeID"),
                        L("SalaryYear"),
                        L("SalaryMonth"),
                        L("DeductionDate"),
                        L("Amount"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, employeeDeductions,
                        _ => _.EmployeeDeductions.DeductionID,
                        _ => _.EmployeeDeductions.EmployeeID,
                        _ => _.EmployeeDeductions.SalaryYear,
                        _ => _.EmployeeDeductions.SalaryMonth,
                        _ => _timeZoneConverter.Convert(_.EmployeeDeductions.DeductionDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.EmployeeDeductions.Amount,
                        _ => _.EmployeeDeductions.Active,
                        _ => _.EmployeeDeductions.AudtUser,
                        _ => _timeZoneConverter.Convert(_.EmployeeDeductions.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.EmployeeDeductions.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.EmployeeDeductions.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var deductionDateColumn = sheet.Column(5);
                    deductionDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					deductionDateColumn.AutoFit();
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
