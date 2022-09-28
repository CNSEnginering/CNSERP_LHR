using Abp.Dependency;
using ERP.PayRoll.EmployeeSalary.Importing.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.EmployeeSalary.Importing
{
    public interface IEmployeeSalaryExcelDataReader : ITransientDependency
    {
        List<ImportEmployeeSalaryDto> GetEmployeesSalaryFromExcel(byte[] fileBytes);
    
    }
}
