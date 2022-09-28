using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.SubDesignations.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.SubDesignations.Exporting
{
    public class SubDesignationsExcelExporter : EpPlusExcelExporterBase, ISubDesignationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SubDesignationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSubDesignationsForViewDto> subDesignations)
        {
            return CreateExcelPackage(
                "SubDesignations.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("SubDesignations"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("SubDesignationID"),
                        L("SubDesignation"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, subDesignations,
                        _ => _.SubDesignations.SubDesignationID,
                        _ => _.SubDesignations.SubDesignation,
                        _ => _.SubDesignations.Active,
                        _ => _.SubDesignations.AudtUser,
                        _ => _timeZoneConverter.Convert(_.SubDesignations.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.SubDesignations.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.SubDesignations.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
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
