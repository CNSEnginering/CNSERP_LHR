using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using OfficeOpenXml;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Importing.Dto;
using ERP.SupplyChain.Inventory.IC_Segment1.Importing.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment1.Importing
{
    public class ICSegment1ListExcelDataReader : EpPlusExcelImporterBase<ImportICSegment1Dto>, IICSegment1ListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;

        public ICSegment1ListExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        public List<ImportICSegment1Dto> GetICSegment1FromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private ImportICSegment1Dto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var ICSegment1 = new ImportICSegment1Dto();

            try
            {
                ICSegment1.Seg1ID = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(ICSegment1.Seg1ID), exceptionMessage);
                ICSegment1.Seg1Name = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(ICSegment1.Seg1Name), exceptionMessage);
            }
            catch (System.Exception exception)
            {
                ICSegment1.Exception = exception.Message;
            }

            return ICSegment1;
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
