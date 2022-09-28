using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.Exporting
{
    public class GLLocationsExcelExporter : EpPlusExcelExporterBase, IGLLocationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public GLLocationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetGLLocationForViewDto> glLocations)
        {
            return CreateExcelPackage(
                "GLLocations.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("GLLocations"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("LocDesc"),
                        L("AuditUser"),
                        L("AuditDate"),
                        L("LocId")
                        );

                    AddObjects(
                        sheet, 2, glLocations,
                        _ => _.GLLocation.LocDesc,
                        _ => _.GLLocation.AuditUser,
                        _ => _timeZoneConverter.Convert(_.GLLocation.AuditDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.GLLocation.LocId
                        );

					var auditDateColumn = sheet.Column(3);
                    auditDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					auditDateColumn.AutoFit();
					

                });
        }
    }
}
