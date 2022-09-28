using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.Allowances.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.Allowances.Exporting
{
    public class AllowancesDetailsExcelExporter : EpPlusExcelExporterBase, IAllowancesDetailsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AllowancesDetailsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAllowancesDetailForViewDto> allowancesDetails)
        {
            return CreateExcelPackage(
                "AllowancesDetails.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("AllowancesDetails"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DetID"),
                        L("EmployeeID"),
                        L("AllowanceType"),
                        L("AllowanceAmt"),
                        L("AllowanceQty"),
                        L("Amount"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, allowancesDetails,
                        _ => _.AllowancesDetail.DetID,
                        _ => _.AllowancesDetail.EmployeeID,
                        _ => _.AllowancesDetail.AllowanceType,
                        _ => _.AllowancesDetail.AllowanceAmt,
                        _ => _.AllowancesDetail.AllowanceQty,
                        _ => _.AllowancesDetail.Amount,
                        _ => _.AllowancesDetail.AudtUser,
                        _ => _timeZoneConverter.Convert(_.AllowancesDetail.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.AllowancesDetail.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.AllowancesDetail.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

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
