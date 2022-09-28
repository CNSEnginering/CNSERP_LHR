using Abp.Localization;
using Abp.Localization.Sources;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.Employees.Importing.Dto;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.Employees.Importing
{
  public  class EmployeeListExcelDataReader : EpPlusExcelImporterBase<ImportEmployeeDto>, IEmployeeListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;
        public EmployeeListExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);

        }

        public List<ImportEmployeeDto> GetEmployeesFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private ImportEmployeeDto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var employee = new ImportEmployeeDto();

            try
            {

                employee.EmployeeID = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(employee.EmployeeID), exceptionMessage)) ?Convert.ToInt32(GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(employee.EmployeeID), exceptionMessage)) : (int?)null ;
                employee.EmployeeName = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(employee.EmployeeName), exceptionMessage);
                employee.FatherName= GetRequiredValueFromRowOrNull(worksheet, row, 3, employee.FatherName, exceptionMessage);
                employee.date_of_birth= !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(employee.date_of_birth), exceptionMessage)) ?DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(employee.date_of_birth), exceptionMessage)) : (DateTime?)null;
                employee.home_address = GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(employee.home_address), exceptionMessage);
                employee.PhoneNo = GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(employee.PhoneNo), exceptionMessage);
                employee.NTN = GetRequiredValueFromRowOrNull(worksheet, row, 7, nameof(employee.NTN), exceptionMessage);
                employee.apointment_date =!string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 8, nameof(employee.apointment_date), exceptionMessage)) ?DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 8, nameof(employee.apointment_date), exceptionMessage)) : (DateTime?)null;
                employee.date_of_joining =!string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 9, nameof(employee.date_of_joining), exceptionMessage)) ? DateTime.Parse( GetRequiredValueFromRowOrNull(worksheet, row, 9, nameof(employee.date_of_joining), exceptionMessage)) : (DateTime?)null;
                employee.date_of_leaving = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 10, nameof(employee.date_of_leaving), exceptionMessage)) ? DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 10, nameof(employee.date_of_leaving), exceptionMessage)) : (DateTime?)null;
                employee.City = GetRequiredValueFromRowOrNull(worksheet, row, 11, nameof(employee.City), exceptionMessage);
                employee.Cnic = GetRequiredValueFromRowOrNull(worksheet, row, 12, nameof(employee.Cnic), exceptionMessage);            
                employee.EdID = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 13, nameof(employee.EdID), exceptionMessage)) ?Convert.ToInt32(GetRequiredValueFromRowOrNull(worksheet, row, 13, nameof(employee.EdID), exceptionMessage)) : 1;
                employee.DeptID = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 14, nameof(employee.DeptID), exceptionMessage)) ?Convert.ToInt32(GetRequiredValueFromRowOrNull(worksheet, row, 14, nameof(employee.DeptID), exceptionMessage)) : (int?)null;
                employee.DesignationID = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 15, nameof(employee.DesignationID), exceptionMessage)) ?Convert.ToInt32(GetRequiredValueFromRowOrNull(worksheet, row, 15, nameof(employee.DesignationID), exceptionMessage)) : (int?)null;
                employee.Gender =GetRequiredValueFromRowOrNull(worksheet, row, 16, nameof(employee.Gender), exceptionMessage);
                employee.Status =!string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 17, nameof(employee.Status), exceptionMessage))? (GetRequiredValueFromRowOrNull(worksheet, row, 17, nameof(employee.Status), exceptionMessage) == "0"?false:true) : (bool?)null;
                employee.ShiftID = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 18, nameof(employee.ShiftID), exceptionMessage)) ?Convert.ToInt32(GetRequiredValueFromRowOrNull(worksheet, row, 18, nameof(employee.ShiftID), exceptionMessage)) : (int?)null;
                employee.TypeID = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 19, nameof(employee.TypeID), exceptionMessage)) ?Convert.ToInt32(GetRequiredValueFromRowOrNull(worksheet, row, 19, nameof(employee.TypeID), exceptionMessage)) : (int?)null;
                employee.SecID = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 20, nameof(employee.SecID), exceptionMessage)) ?Convert.ToInt32(GetRequiredValueFromRowOrNull(worksheet, row, 20, nameof(employee.SecID), exceptionMessage)) : 1;
                employee.ReligionID = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 21, nameof(employee.ReligionID), exceptionMessage)) ?Convert.ToInt32(GetRequiredValueFromRowOrNull(worksheet, row, 21, nameof(employee.ReligionID), exceptionMessage)) : (int?)null;
                employee.social_security =GetRequiredValueFromRowOrNull(worksheet, row, 22, nameof(employee.social_security), exceptionMessage) == "0" ? false : true;
                employee.eobi =GetRequiredValueFromRowOrNull(worksheet, row, 23, nameof(employee.social_security), exceptionMessage) == "0" ? false : true;
                employee.wppf =GetRequiredValueFromRowOrNull(worksheet, row, 24, nameof(employee.wppf), exceptionMessage) == "0" ? false : true;
                employee.payment_mode =!string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 25, nameof(employee.payment_mode), exceptionMessage)) ?( GetRequiredValueFromRowOrNull(worksheet, row, 25, nameof(employee.payment_mode), exceptionMessage)=="Cash" ? "0" : (GetRequiredValueFromRowOrNull(worksheet, row, 25, nameof(employee.payment_mode), exceptionMessage) != "Bank" ? "1" : "2")) :null ;
                employee.bank_name = GetRequiredValueFromRowOrNull(worksheet, row, 26, nameof(employee.bank_name), exceptionMessage);
                employee.account_no = GetRequiredValueFromRowOrNull(worksheet, row, 27, nameof(employee.account_no), exceptionMessage);
                employee.academic_qualification = GetRequiredValueFromRowOrNull(worksheet, row, 28, nameof(employee.academic_qualification), exceptionMessage);
                employee.professional_qualification = GetRequiredValueFromRowOrNull(worksheet, row, 29, nameof(employee.professional_qualification), exceptionMessage);
                employee.first_rest_days =Convert.ToInt32(GetRequiredValueFromRowOrNull(worksheet, row, 30, nameof(employee.first_rest_days), exceptionMessage));
                employee.first_rest_days_w2 =Convert.ToInt32(GetRequiredValueFromRowOrNull(worksheet, row,31, nameof(employee.first_rest_days_w2), exceptionMessage));
                employee.second_rest_days =Convert.ToInt32(GetRequiredValueFromRowOrNull(worksheet, row, 32, nameof(employee.second_rest_days), exceptionMessage));
                employee.second_rest_days_w2 =Convert.ToInt32(GetRequiredValueFromRowOrNull(worksheet, row, 33, nameof(employee.second_rest_days_w2), exceptionMessage));
                employee.BloodGroup =GetRequiredValueFromRowOrNull(worksheet, row, 34, nameof(employee.BloodGroup), exceptionMessage);
                employee.Reference =GetRequiredValueFromRowOrNull(worksheet, row, 35, nameof(employee.Reference), exceptionMessage);
                employee.Visa_Details =GetRequiredValueFromRowOrNull(worksheet, row, 36, nameof(employee.Visa_Details), exceptionMessage);
                employee.Driving_Licence =GetRequiredValueFromRowOrNull(worksheet, row, 37, nameof(employee.Driving_Licence), exceptionMessage);
                employee.Duty_Hours =Convert.ToDouble(GetRequiredValueFromRowOrNull(worksheet, row, 38, nameof(employee.Duty_Hours), exceptionMessage));
                employee.Active =!string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 39, nameof(employee.Active), exceptionMessage)) ?( GetRequiredValueFromRowOrNull(worksheet, row, 39, nameof(employee.Active), exceptionMessage) == "0" ? false : true) : (bool?)null;
                employee.Confirmed =  !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 40, nameof(employee.Confirmed), exceptionMessage)) ? (GetRequiredValueFromRowOrNull(worksheet, row, 40, nameof(employee.Confirmed), exceptionMessage) == "0" ? false : true) : (bool?)null;
                employee.OldEmployeeID = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 41, nameof(employee.OldEmployeeID), exceptionMessage)) ? Convert.ToInt32(GetRequiredValueFromRowOrNull(worksheet, row, 41, nameof(employee.OldEmployeeID), exceptionMessage)) : 0;
                employee.EoBiNo = GetRequiredValueFromRowOrNull(worksheet, row, 42, nameof(employee.EoBiNo), exceptionMessage);
                employee.SscNo = GetRequiredValueFromRowOrNull(worksheet, row, 43, nameof(employee.SscNo), exceptionMessage);
                employee.MStatus = GetRequiredValueFromRowOrNull(worksheet, row, 44, nameof(employee.MStatus), exceptionMessage);
                employee.ContractExpDate = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 45, nameof(employee.ContractExpDate), exceptionMessage)) ? DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 45, nameof(employee.ContractExpDate), exceptionMessage)) :(DateTime?)null;
            }
            catch (System.Exception exception)
            {
                employee.Exception = exception.Message;
            }

            return employee;
        }

        private string GetRequiredValueFromRowOrNull(ExcelWorksheet worksheet, int row, int column, string columnName, StringBuilder exceptionMessage)
        {
            var cellValue = worksheet.Cells[row, column].Text;

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
