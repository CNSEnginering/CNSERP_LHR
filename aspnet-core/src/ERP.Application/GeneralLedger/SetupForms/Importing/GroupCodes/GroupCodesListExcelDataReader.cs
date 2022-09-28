using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using OfficeOpenXml;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Importing.Dto;
using ERP.GeneralLedger.SetupForms.Importing.GroupCodes.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.GroupCodes
{
    public class GroupCodesListExcelDataReader : EpPlusExcelImporterBase<ImportGroupCodesDto>, IGroupCodesListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;

        public GroupCodesListExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        public List<ImportGroupCodesDto> GetGroupCodesFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private ImportGroupCodesDto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var GroupCodes = new ImportGroupCodesDto();

            try
            {
                GroupCodes.Seg1ID = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(GroupCodes.Seg1ID), exceptionMessage);
                GroupCodes.SegmentName = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(GroupCodes.Seg1ID), exceptionMessage);
                GroupCodes.Family = int.Parse( GetRequiredValueFromRowOrNull(worksheet, row, 3, GroupCodes.Seg1ID, exceptionMessage));
                GroupCodes.OldCode = GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(GroupCodes.Seg1ID), exceptionMessage);
            }
            catch (System.Exception exception)
            {
                GroupCodes.Exception = exception.Message;
            }

            return GroupCodes;
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
