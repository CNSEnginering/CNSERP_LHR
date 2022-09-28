using Abp.Domain.Repositories;
using ERP.PayRoll.EmployeeArrears;
using ERP.PayRoll.Designation;
using ERP.PayRoll.Shifts;
using ERP.Reports.PayRoll.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.PayRoll.Department;
using ERP.PayRoll.Employees;
using Abp.Authorization;
using ERP.Authorization;

namespace ERP.Reports.PayRoll
{
    [AbpAuthorize(AppPermissions.PayRollReports_EmployeeReports)]
    public class EmployeeArrearsTransactionAppService : ERPReportAppServiceBase, IEmployeeArrearsTransactionAppService
    {
        private readonly IRepository<EmployeeArrears> _employeeArrearsRepository;
        private readonly IRepository<Employees> _employeeRepository;
        private readonly IRepository<Designations> _designationRepository;
        private readonly IRepository<Department> _deparmentRepository;

        public EmployeeArrearsTransactionAppService(
            IRepository<EmployeeArrears> employeeArrearsRepository,
            IRepository<Employees> employeeRepository,
            IRepository<Designations> designationRepository,
            IRepository<Department> deparmentRepository)
        {
            _employeeArrearsRepository = employeeArrearsRepository;
            _employeeRepository = employeeRepository;
            _designationRepository = designationRepository;
            _deparmentRepository = deparmentRepository;
        }
        public List<EmployeeArrearsTransactionDto> GetData(int? TenantId, string fromEmpID, string toEmpID, string fromDeptID, string toDeptID,
            string fromSecID, string toSecID, string fromSalary, string toSalary, short SalaryYear, short SalaryMonth)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            var employeeArrears = _employeeArrearsRepository.GetAll().Where(o => o.TenantId == TenantId && o.SalaryMonth == SalaryMonth && o.SalaryYear == SalaryYear);

            var employee = _employeeRepository.GetAll().Where(o => o.TenantId == TenantId &&
           o.EmployeeID >= Convert.ToInt32(fromEmpID) && o.EmployeeID <= Convert.ToInt32(toEmpID) &&
           o.DeptID >= Convert.ToInt32(fromDeptID) && o.DeptID <= Convert.ToInt32(toDeptID) &&
           o.SecID >= Convert.ToInt32(fromSecID) && o.SecID <= Convert.ToInt32(toSecID));

            var department = _deparmentRepository.GetAll().Where(o => o.TenantId == TenantId);
            var designation = _designationRepository.GetAll().Where(o => o.TenantId == TenantId);

            var data = from a in employeeArrears
                       join b in employee on a.EmployeeID equals b.EmployeeID
                       join c in department on b.DeptID equals c.DeptID
                       join d in designation on b.DesignationID equals d.DesignationID
                       where (b.Active == true)
                       select new EmployeeArrearsTransactionDto()
                       {
                           //SalaryMonthYear = (a.SalaryMonth>9? a.SalaryMonth.ToString(): "0"+ a.SalaryMonth.ToString() )+ "/"+ a.SalaryYear.ToString(),
                           SalaryMonthYear = a.ArrearDate.Value.ToString("MMMM/yyyy"),
                           EmployeeID = a.EmployeeID.ToString(),
                           EmployeeName = b.EmployeeName,
                           DeptName = c.DeptName,
                           Designation = d.Designation,
                           Amount = a.Amount.ToString()
                       };

            return data.ToList();
        }

    }
}
