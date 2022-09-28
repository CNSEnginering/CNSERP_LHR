using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.Exporting
{
    public class BatchListPreviewsExcelExporter : EpPlusExcelExporterBase, IBatchListPreviewsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BatchListPreviewsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBatchListPreviewForViewDto> batchListPreviews)
        {
            return CreateExcelPackage(
                "BatchListPreviews.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("BatchListPreviews"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DocDate"),
                        L("Description"),
                        L("BookDesc"),
                        L("Debit"),
                        L("Credit")
                        );

                    AddObjects(
                        sheet, 2, batchListPreviews,
                        _ => _.BatchListPreview.DocDate,
                        _ => _.BatchListPreview.Description,
                        _ => _.BatchListPreview.BookDesc,
                        _ => _.BatchListPreview.Debit,
                        _ => _.BatchListPreview.Credit
                        );

					

                });
        }
    }
}
