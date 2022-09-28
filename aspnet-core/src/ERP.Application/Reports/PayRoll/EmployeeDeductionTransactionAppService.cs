using Abp.Domain.Repositories;
using ERP.PayRoll.Department;
using ERP.PayRoll.Designation;
using ERP.PayRoll.EmployeeDeductions;
using ERP.PayRoll.Employees;
using ERP.Reports.PayRoll.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Authorization;
using ERP.Authorization;
using ERP.PayRoll.DeductionTypes;

namespace ERP.Reports.PayRoll
{
    [AbpAuthorize(AppPermissions.PayRollReports_EmployeeReports)]
    public class EmployeeDeductionTransactionAppService : ERPReportAppServiceBase, IEmployeeDeductionTransactionAppService
    {
        private readonly IRepository<EmployeeDeductions> _employeeDeductionsRepository;
        private readonly IRepository<DeductionTypes> _deductionTypesRepository;
        private readonly IRepository<Employees> _employeesRepository;
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<Designations> _designationRepository;



        public EmployeeDeductionTransactionAppService(IRepository<EmployeeDeductions> employeeDeductionsRepository, IRepository<Employees> employeesRepository,
            IRepository<Department> departmentRepository, IRepository<Designations> designationRepository,
            IRepository<DeductionTypes> deductionTypesRepository)
        {
            _employeeDeductionsRepository = employeeDeductionsRepository;
            _employeesRepository = employeesRepository;
            _departmentRepository = departmentRepository;
            _designationRepository = designationRepository;
            _deductionTypesRepository = deductionTypesRepository;
        }

        public List<EmployeeDeductionTransactionDto> GetData(int? TenantId, string fromEmpID, string toEmpID, string fromDeptID, string toDeptID,
            string fromSecID, string toSecID, string fromSalary, string toSalary, short SalaryMonth, short SalaryYear)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }

            var employeeEarningsData = _employeeDeductionsRepository.GetAll().Where(e => e.SalaryMonth == SalaryMonth && e.SalaryYear == SalaryYear && e.TenantId == TenantId);
            var deductionTypes = _deductionTypesRepository.GetAll().Where(e => e.TenantId == TenantId);
            var employeesData = _employeesRepository.GetAll().Where(o => o.TenantId == TenantId &&
            o.EmployeeID >= Convert.ToInt32(fromEmpID) && o.EmployeeID <= Convert.ToInt32(toEmpID) &&
            o.DeptID >= Convert.ToInt32(fromDeptID) && o.DeptID <= Convert.ToInt32(toDeptID) &&
            o.SecID >= Convert.ToInt32(fromSecID) && o.SecID <= Convert.ToInt32(toSecID));

            var departmentData = _departmentRepository.GetAll().Where(o => o.TenantId == TenantId);
            var designationData = _designationRepository.GetAll().Where(o => o.TenantId == TenantId);

            var data = from a in employeeEarningsData
                       join b in employeesData
                       on a.EmployeeID equals b.EmployeeID
                       join c in departmentData
                       on b.DeptID equals c.DeptID
                       join d in designationData
                       on b.DesignationID equals d.DesignationID
                       join e in deductionTypes
                       on a.DeductionType equals e.TypeID
                       select new EmployeeDeductionTransactionDto()
                       {
                           EmployeeID = a.EmployeeID.ToString(),
                           EmployeeName = b.EmployeeName,
                           DeptName = c.DeptName,
                           Designation = d.Designation,
                           Amount = a.Amount.ToString(),
                           DeductionDate = a.DeductionDate.Value.ToString("MMMM/yyyy"),
                           DeductionType = e.TypeDesc
                       };


            return data.ToList();
        }
    }

}
