using Abp.Domain.Repositories;
using ERP.PayRoll.Department;
using ERP.PayRoll.Designation;
using ERP.PayRoll.Employees;
using ERP.PayRoll.Location;
using ERP.PayRoll.SalarySheet;
using ERP.Reports.PayRoll.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.PayRoll
{
    public class SalarySheetSummaryAppService : ERPReportAppServiceBase, ISalarySheetSummaryAppService
    {
        private readonly IRepository<SalarySheet> _salarySheetRepository;
        private readonly IRepository<Employees> _employeesRepository;

        private readonly IRepository<Designations> _designationRepository;
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<Locations> _locationRepository;

        public SalarySheetSummaryAppService(IRepository<SalarySheet> salarySheetRepository, IRepository<Employees> employeesRepository, IRepository<Designations> designationRepository, IRepository<Department> departmentRepository
            ,
             IRepository<Locations> locationRepository)
        {
            _salarySheetRepository = salarySheetRepository;
            _employeesRepository = employeesRepository;
            _designationRepository = designationRepository;
            _departmentRepository = departmentRepository;
            _locationRepository = locationRepository;
        }

        public List<SalarySheetSummaryDto> GetData(int? TenantId, short SalaryMonth, short SalaryYear)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }

            var salarySheetData = _salarySheetRepository.GetAll().Where(e => e.SalaryMonth == SalaryMonth && e.SalaryYear == SalaryYear && e.TenantId == TenantId);


            var employeesData = _employeesRepository.GetAll().Where(o => o.TenantId == TenantId);

            var locations = _locationRepository.GetAll().Where(o => o.TenantId == TenantId);

            var data = from a in salarySheetData
                       join b in employeesData
                       on a.EmployeeID equals b.EmployeeID
                       join l in locations
                       on b.LocID equals l.LocID
                       join c in _designationRepository.GetAll().Where(o => o.TenantId == TenantId)
                       on b.DesignationID equals c.DesignationID
                       join d in _departmentRepository.GetAll().Where(o => o.TenantId == TenantId)
                       on b.DeptID equals d.DeptID

                       where a.work_days != 0 
                       orderby a.EmployeeID


                       select new SalarySheetSummaryDto()
                       {
                           EmployeeID = a.EmployeeID.ToString(),
                           EmployeeName = b.EmployeeName,
                           Amount = a.total_earnings.Value.ToString("N0"),
                           Designation = c.Designation,
                           Department = d.DeptName,
                           SalaryMonth = a.SalaryMonth,
                           SalaryYear = a.SalaryYear,
                           
                           //ModOfPay = a.ModOfPay.ToString()
                           ModOfPay = (a.ModOfPay == 0) ? "Cash" : (a.ModOfPay == 1) ? "Bank Advice"
                           : (a.ModOfPay == 2) ? "Cheque" : (a.ModOfPay == 3) ? "Stopped" : "",
                           Location = l.Location
                       }; 

            //data = data
            // .GroupBy(e => e.EmployeeID)
            // .Select(a => a.OrderByDescending(g => g.gross_salary).First());

            //var temp = data.GroupBy(o=>o.ModOfPay);

            return data.OrderBy(o => int.Parse(o.EmployeeID)).ToList();
        }
    }
}
