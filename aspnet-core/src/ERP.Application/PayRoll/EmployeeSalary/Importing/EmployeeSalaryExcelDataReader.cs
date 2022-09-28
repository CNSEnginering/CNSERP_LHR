using Abp.Localization;
using Abp.Localization.Sources;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.EmployeeSalary.Importing.Dto;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.EmployeeSalary.Importing
{
  public  class EmployeeSalaryExcelDataReader : EpPlusExcelImporterBase<ImportEmployeeSalaryDto>, IEmployeeSalaryExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;
        public EmployeeSalaryExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);

        }

        public List<ImportEmployeeSalaryDto> GetEmployeesSalaryFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private ImportEmployeeSalaryDto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var employee = new ImportEmployeeSalaryDto();

            try
            {

                employee.EmployeeID = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(employee.EmployeeID), exceptionMessage)) ? Convert.ToInt32(GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(employee.EmployeeID), exceptionMessage)) : 0;
                employee.EmployeeName = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(employee.EmployeeName), exceptionMessage);
                employee.StartDate =Convert.ToDateTime(GetRequiredValueFromRowOrNull(worksheet, row, 3, nameof(employee.StartDate), exceptionMessage));
                employee.Gross_Salary = Convert.ToDouble(GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(employee.Gross_Salary), exceptionMessage));
                employee.Basic_Salary= Convert.ToDouble(GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(employee.Basic_Salary), exceptionMessage));
                employee.Net_Salary = Convert.ToDouble(GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(employee.Net_Salary), exceptionMessage));
                employee.Tax = Convert.ToDouble(GetRequiredValueFromRowOrNull(worksheet, row, 7, nameof(employee.Tax), exceptionMessage));


            }
            catch (System.Exception exception)
            {
                employee.Exception = exception.Message;
            }

            return employee;
        }

        private string GetRequiredValueFromRowOrNull(ExcelWorksheet worksheet, int row, int column, string columnName, StringBuilder exceptionMessage)
        {
            var cellValue = worksheet.Cells[row, column].Value;

            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
            {
                return cellValue.ToString();
            }

            //exceptionMessage.Append(GetLocalizedExceptionMessagePart(columnName));
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
