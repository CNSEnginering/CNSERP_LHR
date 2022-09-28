using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.Holidays.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.Holidays.Exporting
{
    public class HolidaysExcelExporter : EpPlusExcelExporterBase, IHolidaysExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HolidaysExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHolidaysForViewDto> holidays)
        {
            return CreateExcelPackage(
                "Holidays.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Holidays"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("HolidayID"),
                        L("HolidayDate"),
                        L("HolidayName"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, holidays,
                        _ => _.Holidays.HolidayID,
                        _ => _timeZoneConverter.Convert(_.Holidays.HolidayDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Holidays.HolidayName,
                        _ => _.Holidays.Active,
                        _ => _.Holidays.AudtUser,
                        _ => _timeZoneConverter.Convert(_.Holidays.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Holidays.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.Holidays.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var holidayDateColumn = sheet.Column(2);
                    holidayDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					holidayDateColumn.AutoFit();
					var audtDateColumn = sheet.Column(6);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
					var createDateColumn = sheet.Column(8);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createDateColumn.AutoFit();
					
					
                });
        }
    }
}
