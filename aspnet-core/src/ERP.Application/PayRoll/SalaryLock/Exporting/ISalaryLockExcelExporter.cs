using System.Collections.Generic;
using ERP.PayRoll.SalaryLock.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.SalaryLock.Exporting
{
    public interface ISalaryLockExcelExporter
    {
        FileDto ExportToFile(List<GetSalaryLockForViewDto> salaryLock);
    }
}