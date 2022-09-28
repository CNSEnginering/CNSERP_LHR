using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.Allowances.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.Allowances.Exporting
{
    public class AllowancesExcelExporter : EpPlusExcelExporterBase, IAllowancesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AllowancesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAllowancesForViewDto> allowances)
        {
            return CreateExcelPackage(
                "Allowances.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Allowances"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DocID"),
                        L("Docdate"),
                        L("DocMonth"),
                        L("DocYear"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, allowances,
                        _ => _.Allowances.DocID,
                        _ => _timeZoneConverter.Convert(_.Allowances.Docdate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Allowances.DocMonth,
                        _ => _.Allowances.DocYear,
                        _ => _.Allowances.AudtUser,
                        _ => _timeZoneConverter.Convert(_.Allowances.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Allowances.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.Allowances.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var docdateColumn = sheet.Column(2);
                    docdateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					docdateColumn.AutoFit();
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
