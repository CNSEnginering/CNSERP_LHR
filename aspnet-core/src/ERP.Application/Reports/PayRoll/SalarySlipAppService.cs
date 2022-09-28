using Abp.Domain.Repositories;
using ERP.PayRoll.Department;
using ERP.PayRoll.Designation;
using ERP.PayRoll.Employees;
using ERP.PayRoll.EmployeeSalary;
using ERP.PayRoll.SalarySheet;
using ERP.Reports.PayRoll.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Authorization;
using ERP.Authorization;
using ERP.PayRoll.EmployeeLoans;

namespace ERP.Reports.PayRoll
{
    [AbpAuthorize(AppPermissions.PayRollReports_SalaryReports)]
    public class SalarySlipAppService : ERPReportAppServiceBase, ISalarySlipAppService
    {
        private readonly IRepository<SalarySheet> _salarySheetRepository;
        private readonly IRepository<Employees> _employeesRepository;
        private readonly IRepository<Designations> _designationRepository;
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<EmployeeSalary> _employeeSalaryRepository;
        private readonly string[] Months = new string[] { "", "January", "Feburary", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };




        public SalarySlipAppService(IRepository<SalarySheet> salarySheetRepository, IRepository<Employees> employeesRepository,
            IRepository<Designations> designationRepository, IRepository<Department> departmentRepository, IRepository<EmployeeSalary> employeeSalaryRepository)
        {
            _salarySheetRepository = salarySheetRepository;
            _employeesRepository = employeesRepository;
            _designationRepository = designationRepository;
            _departmentRepository = departmentRepository;
            _employeeSalaryRepository = employeeSalaryRepository;

        }

        public List<SalarySlipDto> GetData(int? TenantId, string fromEmpID, string toEmpID, string fromDeptID, string toDeptID,
            string fromSecID, string toSecID, string fromSalary, string toSalary, short SalaryMonth, short SalaryYear)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }

            var salarySheetData = _salarySheetRepository.GetAll().Where(e => e.SalaryMonth == SalaryMonth && e.SalaryYear == SalaryYear && e.TenantId == TenantId);

            var employeesData = _employeesRepository.GetAll().Where(o => o.TenantId == TenantId &&
            o.EmployeeID >= Convert.ToInt32(fromEmpID) && o.EmployeeID <= Convert.ToInt32(toEmpID) &&
            o.DeptID >= Convert.ToInt32(fromDeptID) && o.DeptID <= Convert.ToInt32(toDeptID) &&
            o.SecID >= Convert.ToInt32(fromSecID) && o.SecID <= Convert.ToInt32(toSecID));

            var designationData = _designationRepository.GetAll().Where(o => o.TenantId == TenantId);
            //var employeeSalaryData = _employeeSalaryRepository.GetAll().Where(o => o.TenantId == TenantId);
            var departmentData = _departmentRepository.GetAll().Where(o => o.TenantId == TenantId);

            var data = from a in salarySheetData
                       join b in employeesData
                       on a.EmployeeID equals b.EmployeeID
                       join c in designationData
                       on b.DesignationID equals c.DesignationID
                       join e in departmentData
                       on b.DeptID equals e.DeptID
                       orderby a.EmployeeID
                       select new SalarySlipDto()
                       {
                           EmployeeID = a.EmployeeID.ToString(),
                           EmployeeName = b.EmployeeName,
                           Designation = c.Designation,
                           DeptName = e.DeptName,
                           CNIC = b.Cnic,
                           Lunch = a.Deduction1,
                           OverTime = a.Income2,
                           work_days = a.work_days ?? 0,
                           basic_earned = a.basic_earned ?? 0,
                           gross_salary = a.gross_salary ?? 0,
                           basic_salary = a.basic_salary ?? 0,
                           total_earnings = a.total_earnings ?? 0,
                           house_rent = a.house_rent ?? 0,
                           arrears = a.arrears ?? 0,
                           tax_amount = a.tax_amount ?? 0,
                           eobi_amount = a.eobi_amount ?? 0,
                           social_security_amount = a.social_security_amount ?? 0,
                           loan = a.loan ?? 0,
                           advance = a.advance ?? 0,
                           other_deductions = a.other_deductions ?? 0,
                           absent_amount = a.absent_amount ?? 0,
                           medical = ((a.basic_earned ?? 0) / 110) * 10,
                           daysInMonth = DateTime.DaysInMonth(SalaryYear, SalaryMonth),
                           monthName = Months[SalaryMonth],
                           employeeType = b.TypeID,
                           joining_date = b.date_of_joining == null ? "01/01/0001" :((DateTime)b.date_of_joining).ToString("dd MMMM yyyy")
                       };

            return data.ToList();
        }
    }
}
