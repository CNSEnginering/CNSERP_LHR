using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.SupplyChain.Sales.SalesReference.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.SupplyChain.Sales.SalesReference.Exporting
{
    public class SalesReferencesExcelExporter : EpPlusExcelExporterBase, ISalesReferencesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SalesReferencesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSalesReferenceForViewDto> salesReferences)
        {
            return CreateExcelPackage(
                "SalesReferences.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("SalesReferences"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("RefID"),
                        L("RefName"),
                        L("ACTIVE"),
                        L("AUDTDATE"),
                        L("AUDTUSER"),
                        L("CreatedDATE"),
                        L("CreatedUSER")
                        );

                    AddObjects(
                        sheet, 2, salesReferences,
                        _ => _.SalesReference.RefID,
                        _ => _.SalesReference.RefName,
                        _ => _.SalesReference.ACTIVE,
                        _ => _timeZoneConverter.Convert(_.SalesReference.AUDTDATE, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.SalesReference.AUDTUSER,
                        _ => _timeZoneConverter.Convert(_.SalesReference.CreatedDATE, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.SalesReference.CreatedUSER
                        );

					var audtdateColumn = sheet.Column(4);
                    audtdateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtdateColumn.AutoFit();
					var createdDATEColumn = sheet.Column(6);
                    createdDATEColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createdDATEColumn.AutoFit();
					

                });
        }
    }
}
