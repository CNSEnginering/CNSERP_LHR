using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.EarningTypes.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.EarningTypes.Exporting
{
    public class EarningTypesExcelExporter : EpPlusExcelExporterBase, IEarningTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EarningTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEarningTypesForViewDto> earningTypes)
        {
            return CreateExcelPackage(
                "EarningTypes.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("EarningTypes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("EarningTypeID"),
                        L("EarningTypeDesc"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, earningTypes,
                        _ => _.EarningTypes.TypeID,
                        _ => _.EarningTypes.TypeDesc,
                        _ => _.EarningTypes.Active,
                        _ => _.EarningTypes.AudtUser,
                        _ => _timeZoneConverter.Convert(_.EarningTypes.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.EarningTypes.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.EarningTypes.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
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
