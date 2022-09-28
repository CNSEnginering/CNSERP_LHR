using System.Collections.Generic;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using OfficeOpenXml;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Importing.SegmentLevel3.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.SegmentLevel3
{
    public class SegmentLevel3ListExcelDataReader : EpPlusExcelImporterBase<ImportSegmentLevel3Dto>, ISegmentLevel3ListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;

        public SegmentLevel3ListExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        public List<ImportSegmentLevel3Dto> GetSegmentLevel3FromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private ImportSegmentLevel3Dto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var SegmentLevel3 = new ImportSegmentLevel3Dto();

            try
            {
                SegmentLevel3.Seg3ID = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(SegmentLevel3.Seg3ID), exceptionMessage);
                SegmentLevel3.SegmentName = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(SegmentLevel3.SegmentName), exceptionMessage);
                SegmentLevel3.OldCode = GetRequiredValueFromRowOrNull(worksheet, row, 3, nameof(SegmentLevel3.OldCode), exceptionMessage);
            }
            catch (System.Exception exception)
            {
                SegmentLevel3.Exception = exception.Message;
            }

            return SegmentLevel3;
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
