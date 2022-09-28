using Abp.Application.Services;
using ERP.Reports.PayRoll.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.PayRoll
{
    public interface IEmployeeListingAppService : IApplicationService
    {
        List<EmployeeListingDto> GetData(int? TenantId, string fromEmpID, string toEmpID, string fromDeptID, string toDeptID,
            string fromSecID, string toSecID, string fromSalary, string toSalary, bool isActive, string fromlocid, string tolocid, string emptype);
        List<AllowanceEmployeeListingDto> GetDataForAllowance(int? TenantId, string fromEmpID, string toEmpID, string fromDeptID, string toDeptID,
         string fromSecID, string toSecID, string AllowanceYear, string AllowanceMonth, bool isActive, string fromlocid, string tolocid, string Allowtype,string EmpType, string allowanceBtype);
    List<SalarySheetSummaryDto> GetDataForAllowanceDisburseMent(int? TenantId, string fromEmpID, string toEmpID, string fromDeptID, string toDeptID,
         string fromSecID, string toSecID, string AllowanceYear, string AllowanceMonth, bool isActive, string fromlocid, string tolocid, string Allowtype,string EmpType, string allowanceBtype);
        List<AllowanceSummaryDto> GetDataForAllowanceSummary(int? TenantId, string fromEmpID, string toEmpID, string fromDeptID, string toDeptID,
         string fromSecID, string toSecID, string AllowanceYear, string AllowanceMonth, bool isActive, string fromlocid, string tolocid, string Allowtype, string EmpType, string allowanceBtype);
    }
}
