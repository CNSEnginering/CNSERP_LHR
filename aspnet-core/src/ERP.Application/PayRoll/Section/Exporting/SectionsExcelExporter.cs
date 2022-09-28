using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.Section.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.Section.Exporting
{
    public class SectionsExcelExporter : EpPlusExcelExporterBase, ISectionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SectionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSectionForViewDto> sections)
        {
            return CreateExcelPackage(
                "Sections.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Sections"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("SecID"),
                        L("SecName"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, sections,
                        _ => _.Section.SecID,
                        _ => _.Section.SecName,
                        _ => _.Section.Active,
                        _ => _.Section.AudtUser,
                        _ => _timeZoneConverter.Convert(_.Section.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Section.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.Section.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
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
