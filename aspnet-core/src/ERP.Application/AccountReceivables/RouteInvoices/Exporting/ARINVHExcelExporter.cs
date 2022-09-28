using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.AccountReceivables.RouteInvoices.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.AccountReceivables.RouteInvoices.Exporting
{
    public class ARINVHExcelExporter : EpPlusExcelExporterBase, IARINVHExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ARINVHExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetARINVHForViewDto> arinvh)
        {
            return CreateExcelPackage(
                "ARINVH.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ARINVH"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DocNo"),
                        L("DocDate"),
                        L("InvDate"),
                        L("LocID"),
                        L("RoutID"),
                        L("RefNo"),
                        L("SaleTypeID"),
                        L("PaymentOption"),
                        L("Narration"),
                        L("BankID"),
                        L("AccountID"),
                        L("ConfigID"),
                        L("ChequeNo"),
                        L("LinkDetID"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate"),
                        L("Posted"),
                        L("PostedBy"),
                        L("PostedDate"),
                        L("TaxAuth"),
                        L("TaxClass"),
                        L("TaxRate"),
                        L("TaxAccID"),
                        L("TaxAmount"),
                        L("InvAmount")
                        );

                    AddObjects(
                        sheet, 2, arinvh,
                        _ => _.ARINVH.DocNo,
                        _ => _timeZoneConverter.Convert(_.ARINVH.DocDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.ARINVH.InvDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ARINVH.LocID,
                        _ => _.ARINVH.RoutID,
                        _ => _.ARINVH.RefNo,
                        _ => _.ARINVH.SaleTypeID,
                        _ => _.ARINVH.PaymentOption,
                        _ => _.ARINVH.Narration,
                        _ => _.ARINVH.BankID,
                        _ => _.ARINVH.AccountID,
                        _ => _.ARINVH.ConfigID,
                        _ => _.ARINVH.ChequeNo,
                        _ => _.ARINVH.LinkDetID,
                        _ => _.ARINVH.AudtUser,
                        _ => _timeZoneConverter.Convert(_.ARINVH.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ARINVH.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.ARINVH.CreateDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ARINVH.Posted,
                        _ => _.ARINVH.PostedBy,
                        _ => _timeZoneConverter.Convert(_.ARINVH.PostedDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var docDateColumn = sheet.Column(2);
                    docDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					docDateColumn.AutoFit();
					var invDateColumn = sheet.Column(3);
                    invDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					invDateColumn.AutoFit();
					var audtDateColumn = sheet.Column(16);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
					var createDateColumn = sheet.Column(18);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createDateColumn.AutoFit();
					var postedDateColumn = sheet.Column(21);
                    postedDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					postedDateColumn.AutoFit();
					

                });
        }
    }
}
