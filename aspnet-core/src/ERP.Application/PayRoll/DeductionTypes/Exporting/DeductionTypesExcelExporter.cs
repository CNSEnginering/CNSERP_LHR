using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.DeductionTypes.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.DeductionTypes.Exporting
{
    public class DeductionTypesExcelExporter : EpPlusExcelExporterBase, IDeductionTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DeductionTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDeductionTypesForViewDto> deductionTypes)
        {
            return CreateExcelPackage(
                "DeductionTypes.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("DeductionTypes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DeductionTypeID"),
                        L("DeductionTypeDesc"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, deductionTypes,
                        _ => _.DeductionTypes.TypeID,
                        _ => _.DeductionTypes.TypeDesc,
                        _ => _.DeductionTypes.Active,
                        _ => _.DeductionTypes.AudtUser,
                        _ => _timeZoneConverter.Convert(_.DeductionTypes.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.DeductionTypes.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.DeductionTypes.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
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
