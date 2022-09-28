using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.Education.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.Education.Exporting
{
    public class EducationExcelExporter : EpPlusExcelExporterBase, IEducationExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EducationExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEducationForViewDto> education)
        {
            return CreateExcelPackage(
                "Education.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Education"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("EdID"),
                        L("Eduction"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, education,
                        _ => _.Education.EdID,
                        _ => _.Education.Eduction,
                        _ => _.Education.Active,
                        _ => _.Education.AudtUser,
                        _ => _timeZoneConverter.Convert(_.Education.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Education.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.Education.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
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
