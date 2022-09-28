using Abp.Application.Services;
using ERP.Reports.PayRoll.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.PayRoll
{
    public interface ISalarySheetListingAppService : IApplicationService
    {
        List<SalarySheetListingDto> GetData(int? TenantId, string fromEmpID, string toEmpID, string fromDeptID, string toDeptID,
            string fromSecID, string toSecID, string fromSalary, string toSalary, short SalaryMonth, short SalaryYear,string fromLocId,string ToLocId,string Emptype);
    }
}
