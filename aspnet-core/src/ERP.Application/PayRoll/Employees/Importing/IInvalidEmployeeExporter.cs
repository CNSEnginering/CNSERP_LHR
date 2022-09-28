using ERP.Dto;
using ERP.PayRoll.Employees.Importing.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.Employees.Importing
{

    public interface IInvalidEmployeeExporter
    {
        FileDto ExportToFile(List<ImportEmployeeDto> userListDtos);
    }
}
