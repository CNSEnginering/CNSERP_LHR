using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.Exporting
{
    public class FiscalCalendersExcelExporter : EpPlusExcelExporterBase, IFiscalCalendersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FiscalCalendersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFiscalCalenderForViewDto> fiscalCalenders)
        {
            return CreateExcelPackage(
                "FiscalCalenders.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("FiscalCalenders"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Period"),
                        L("StartDate"),
                        L("EndDate"),
                        L("GL"),
                        L("AP"),
                        L("AR"),
                        L("IN"),
                        L("PO"),
                        L("OE"),
                        L("BK"),
                        L("HR"),
                        L("PR"),
                        L("CreatedBy"),
                        L("CreatedDate"),
                        L("EditDate"),
                        L("EditUser")
                        );

                    AddObjects(
                        sheet, 2, fiscalCalenders,
                        _ => _.FiscalCalender.Period,
                        _ => _timeZoneConverter.Convert(_.FiscalCalender.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalender.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.FiscalCalender.GL,
                        _ => _.FiscalCalender.AP,
                        _ => _.FiscalCalender.AR,
                        _ => _.FiscalCalender.IN,
                        _ => _.FiscalCalender.PO,
                        _ => _.FiscalCalender.OE,
                        _ => _.FiscalCalender.BK,
                        _ => _.FiscalCalender.HR,
                        _ => _.FiscalCalender.PR,
                        _ => _.FiscalCalender.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.FiscalCalender.CreatedDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalender.EditDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.FiscalCalender.EditUser
                        );

					var startDateColumn = sheet.Column(2);
                    startDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					startDateColumn.AutoFit();
					var endDateColumn = sheet.Column(3);
                    endDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					endDateColumn.AutoFit();
					var createdDateColumn = sheet.Column(14);
                    createdDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createdDateColumn.AutoFit();
					var editDateColumn = sheet.Column(15);
                    editDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					editDateColumn.AutoFit();
					

                });
        }
    }
}
