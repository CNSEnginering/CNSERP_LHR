using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.EmployeeType.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.EmployeeType.Exporting
{
    public class EmployeeTypeExcelExporter : EpPlusExcelExporterBase, IEmployeeTypeExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeeTypeExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeeTypeForViewDto> employeeType)
        {
            return CreateExcelPackage(
                "EmployeeType.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("EmployeeType"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("TypeID"),
                        L("EmpType"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, employeeType,
                        _ => _.EmployeeType.TypeID,
                        _ => _.EmployeeType.EmpType,
                        _ => _.EmployeeType.Active,
                        _ => _.EmployeeType.AudtUser,
                        _ => _timeZoneConverter.Convert(_.EmployeeType.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.EmployeeType.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.EmployeeType.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
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
