using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using OfficeOpenXml;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Importing.Dto;
using ERP.SupplyChain.Inventory.IC_Segment3.Importing.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment3.Importing
{
    public class ICSegment3ListExcelDataReader : EpPlusExcelImporterBase<ImportICSegment3Dto>, IICSegment3ListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;

        public ICSegment3ListExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        public List<ImportICSegment3Dto> GetICSegment3FromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private ImportICSegment3Dto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var ICSegment3 = new ImportICSegment3Dto();

            try
            {
                ICSegment3.Seg1Id = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(ICSegment3.Seg1Id), exceptionMessage);
                ICSegment3.Seg2Id = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(ICSegment3.Seg2Id), exceptionMessage);
                ICSegment3.Seg3Id =  GetRequiredValueFromRowOrNull(worksheet, row, 3, ICSegment3.Seg1Id, exceptionMessage);
                ICSegment3.Seg3Name = GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(ICSegment3.Seg3Name), exceptionMessage);
            }
            catch (System.Exception exception)
            {
                ICSegment3.Exception = exception.Message;
            }

            return ICSegment3;
        }

        private string GetRequiredValueFromRowOrNull(ExcelWorksheet worksheet, int row, int column, string columnName, StringBuilder exceptionMessage)
        {
            var cellValue = worksheet.Cells[row, column].Value;

            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
            {
                return cellValue.ToString();
            }

            exceptionMessage.Append(GetLocalizedExceptionMessagePart(columnName));
            return null;
        }

        private string GetLocalizedExceptionMessagePart(string parameter)
        {
            return _localizationSource.GetString("{0}IsInvalid", _localizationSource.GetString(parameter)) + "; ";
        }

        private bool IsRowEmpty(ExcelWorksheet worksheet, int row)
        {
            return worksheet.Cells[row, 1].Value == null || string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Value.ToString());
        }
    }
}
