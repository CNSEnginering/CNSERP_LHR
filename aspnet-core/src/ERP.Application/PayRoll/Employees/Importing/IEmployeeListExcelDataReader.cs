using Abp.Dependency;
using ERP.PayRoll.Employees.Importing.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.Employees.Importing
{
    public interface IEmployeeListExcelDataReader : ITransientDependency
    {
        List<ImportEmployeeDto> GetEmployeesFromExcel(byte[] fileBytes);
    }
}
