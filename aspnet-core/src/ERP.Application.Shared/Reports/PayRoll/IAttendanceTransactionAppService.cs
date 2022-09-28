using Abp.Application.Services;
using ERP.Reports.PayRoll.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.PayRoll
{
    public interface IAttendanceTransactionAppService : IApplicationService
    {
        List<AttendanceTransactionDto> GetData(int? TenantId, string fromEmpID, string toEmpID, string fromDeptID, string toDeptID,
            string fromSecID, string toSecID, string fromSalary, string toSalary, short AttendanceYear, short AttendanceMonth);
    }
}
