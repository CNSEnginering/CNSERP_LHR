using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using OfficeOpenXml;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Importing.Dto;
using ERP.GeneralLedger.SetupForms.Importing.GroupCategory.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.GroupCategory
{
    public class GroupCategoryListExcelDataReader : EpPlusExcelImporterBase<ImportGroupCategoryDto>, IGroupCategoryListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;

        public GroupCategoryListExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        public List<ImportGroupCategoryDto> GetGroupCategoryFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private ImportGroupCategoryDto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var GroupCategory = new ImportGroupCategoryDto();

            try
            {
                GroupCategory.Seg1ID = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(GroupCategory.Seg1ID), exceptionMessage);
                GroupCategory.SegmentName = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(GroupCategory.Seg1ID), exceptionMessage);
                GroupCategory.Family = int.Parse( GetRequiredValueFromRowOrNull(worksheet, row, 3, GroupCategory.Seg1ID, exceptionMessage));
                GroupCategory.OldCode = GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(GroupCategory.Seg1ID), exceptionMessage);
            }
            catch (System.Exception exception)
            {
                GroupCategory.Exception = exception.Message;
            }

            return GroupCategory;
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
