using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.CommonServices.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.CommonServices.Exporting
{
    public class FiscalCalendarsExcelExporter : EpPlusExcelExporterBase, IFiscalCalendarsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FiscalCalendarsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFiscalCalendarForViewDto> fiscalCalendars)
        {
            return CreateExcelPackage(
                "FiscalCalendars.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("FiscalCalendars"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("AUDTDATE"),
                        L("AUDTUSER"),
                        L("PERIODS"),
                        L("QTR4PERD"),
                        L("ACTIVE"),
                        L("BGNDATE1"),
                        L("BGNDATE2"),
                        L("BGNDATE3"),
                        L("BGNDATE4"),
                        L("BGNDATE5"),
                        L("BGNDATE6"),
                        L("BGNDATE7"),
                        L("BGNDATE8"),
                        L("BGNDATE9"),
                        L("BGNDATE10"),
                        L("BGNDATE11"),
                        L("BGNDATE12"),
                        L("BGNDATE13"),
                        L("ENDDATE1"),
                        L("ENDDATE2"),
                        L("ENDDATE3"),
                        L("ENDDATE4"),
                        L("ENDDATE5"),
                        L("ENDDATE6"),
                        L("ENDDATE7"),
                        L("ENDDATE8"),
                        L("ENDDATE9"),
                        L("ENDDATE10"),
                        L("ENDDATE11"),
                        L("ENDDATE12"),
                        L("ENDDATE13")
                        );

                    AddObjects(
                        sheet, 2, fiscalCalendars,
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.AUDTDATE, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.FiscalCalendar.AUDTUSER,
                        _ => _.FiscalCalendar.PERIODS,
                        _ => _.FiscalCalendar.QTR4PERD,
                        _ => _.FiscalCalendar.ACTIVE,
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.BGNDATE1, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.BGNDATE2, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.BGNDATE3, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.BGNDATE4, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.BGNDATE5, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.BGNDATE6, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.BGNDATE7, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.BGNDATE8, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.BGNDATE9, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.BGNDATE10, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.BGNDATE11, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.BGNDATE12, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.BGNDATE13, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.ENDDATE1, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.ENDDATE2, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.ENDDATE3, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.ENDDATE4, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.ENDDATE5, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.ENDDATE6, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.ENDDATE7, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.ENDDATE8, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.ENDDATE9, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.ENDDATE10, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.ENDDATE11, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.ENDDATE12, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FiscalCalendar.ENDDATE13, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var audtdateColumn = sheet.Column(1);
                    audtdateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtdateColumn.AutoFit();
					var bgndatE1Column = sheet.Column(6);
                    bgndatE1Column.Style.Numberformat.Format = "yyyy-mm-dd";
					bgndatE1Column.AutoFit();
					var bgndatE2Column = sheet.Column(7);
                    bgndatE2Column.Style.Numberformat.Format = "yyyy-mm-dd";
					bgndatE2Column.AutoFit();
					var bgndatE3Column = sheet.Column(8);
                    bgndatE3Column.Style.Numberformat.Format = "yyyy-mm-dd";
					bgndatE3Column.AutoFit();
					var bgndatE4Column = sheet.Column(9);
                    bgndatE4Column.Style.Numberformat.Format = "yyyy-mm-dd";
					bgndatE4Column.AutoFit();
					var bgndatE5Column = sheet.Column(10);
                    bgndatE5Column.Style.Numberformat.Format = "yyyy-mm-dd";
					bgndatE5Column.AutoFit();
					var bgndatE6Column = sheet.Column(11);
                    bgndatE6Column.Style.Numberformat.Format = "yyyy-mm-dd";
					bgndatE6Column.AutoFit();
					var bgndatE7Column = sheet.Column(12);
                    bgndatE7Column.Style.Numberformat.Format = "yyyy-mm-dd";
					bgndatE7Column.AutoFit();
					var bgndatE8Column = sheet.Column(13);
                    bgndatE8Column.Style.Numberformat.Format = "yyyy-mm-dd";
					bgndatE8Column.AutoFit();
					var bgndatE9Column = sheet.Column(14);
                    bgndatE9Column.Style.Numberformat.Format = "yyyy-mm-dd";
					bgndatE9Column.AutoFit();
					var bgndatE10Column = sheet.Column(15);
                    bgndatE10Column.Style.Numberformat.Format = "yyyy-mm-dd";
					bgndatE10Column.AutoFit();
					var bgndatE11Column = sheet.Column(16);
                    bgndatE11Column.Style.Numberformat.Format = "yyyy-mm-dd";
					bgndatE11Column.AutoFit();
					var bgndatE12Column = sheet.Column(17);
                    bgndatE12Column.Style.Numberformat.Format = "yyyy-mm-dd";
					bgndatE12Column.AutoFit();
					var bgndatE13Column = sheet.Column(18);
                    bgndatE13Column.Style.Numberformat.Format = "yyyy-mm-dd";
					bgndatE13Column.AutoFit();
					var enddatE1Column = sheet.Column(19);
                    enddatE1Column.Style.Numberformat.Format = "yyyy-mm-dd";
					enddatE1Column.AutoFit();
					var enddatE2Column = sheet.Column(20);
                    enddatE2Column.Style.Numberformat.Format = "yyyy-mm-dd";
					enddatE2Column.AutoFit();
					var enddatE3Column = sheet.Column(21);
                    enddatE3Column.Style.Numberformat.Format = "yyyy-mm-dd";
					enddatE3Column.AutoFit();
					var enddatE4Column = sheet.Column(22);
                    enddatE4Column.Style.Numberformat.Format = "yyyy-mm-dd";
					enddatE4Column.AutoFit();
					var enddatE5Column = sheet.Column(23);
                    enddatE5Column.Style.Numberformat.Format = "yyyy-mm-dd";
					enddatE5Column.AutoFit();
					var enddatE6Column = sheet.Column(24);
                    enddatE6Column.Style.Numberformat.Format = "yyyy-mm-dd";
					enddatE6Column.AutoFit();
					var enddatE7Column = sheet.Column(25);
                    enddatE7Column.Style.Numberformat.Format = "yyyy-mm-dd";
					enddatE7Column.AutoFit();
					var enddatE8Column = sheet.Column(26);
                    enddatE8Column.Style.Numberformat.Format = "yyyy-mm-dd";
					enddatE8Column.AutoFit();
					var enddatE9Column = sheet.Column(27);
                    enddatE9Column.Style.Numberformat.Format = "yyyy-mm-dd";
					enddatE9Column.AutoFit();
					var enddatE10Column = sheet.Column(28);
                    enddatE10Column.Style.Numberformat.Format = "yyyy-mm-dd";
					enddatE10Column.AutoFit();
					var enddatE11Column = sheet.Column(29);
                    enddatE11Column.Style.Numberformat.Format = "yyyy-mm-dd";
					enddatE11Column.AutoFit();
					var enddatE12Column = sheet.Column(30);
                    enddatE12Column.Style.Numberformat.Format = "yyyy-mm-dd";
					enddatE12Column.AutoFit();
					var enddatE13Column = sheet.Column(31);
                    enddatE13Column.Style.Numberformat.Format = "yyyy-mm-dd";
					enddatE13Column.AutoFit();
					

                });
        }
    }
}
