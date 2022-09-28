using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using OfficeOpenXml;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Importing.Dto;
using ERP.SupplyChain.Inventory.IC_Segment2.Importing.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment2.Importing
{
    public class ICSegment2ListExcelDataReader : EpPlusExcelImporterBase<ImportICSegment2Dto>, IICSegment2ListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;

        public ICSegment2ListExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        public List<ImportICSegment2Dto> GetICSegment2FromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private ImportICSegment2Dto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var ICSegment2 = new ImportICSegment2Dto();

            try
            {
                ICSegment2.Seg1Id = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(ICSegment2.Seg1Id), exceptionMessage);
                ICSegment2.Seg2Id = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(ICSegment2.Seg2Id), exceptionMessage);
                ICSegment2.Seg2Name = GetRequiredValueFromRowOrNull(worksheet, row, 3, ICSegment2.Seg2Name, exceptionMessage);
                
            }
            catch (System.Exception exception)
            {
                ICSegment2.Exception = exception.Message;
            }

            return ICSegment2;
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
