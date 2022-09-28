using Abp.Domain.Repositories;
using ERP.PayRoll.Department;
using ERP.PayRoll.Designation;
using ERP.PayRoll.Employees;
using ERP.PayRoll.EmployeeSalary;
using ERP.PayRoll.SalarySheet;
using ERP.PayRoll.Section;
using ERP.Reports.PayRoll.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Authorization;
using ERP.Authorization;
using ERP.PayRoll.EarningTypes;
using ERP.PayRoll.EmployeeAdvances;
using ERP.PayRoll.DeductionTypes;
using Abp.Collections.Extensions;
using ERP.PayRoll.Location;
using ERP.PayRoll.EmployeeType;

namespace ERP.Reports.PayRoll
{
    [AbpAuthorize(AppPermissions.PayRollReports_SalaryReports)]
    public class SalarySheetListingAppService : ERPReportAppServiceBase, ISalarySheetListingAppService
    {

        private readonly IRepository<SalarySheet> _salarySheetRepository;
        private readonly IRepository<Employees> _employeesRepository;
        private readonly IRepository<Designations> _designationRepository;
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<EmployeeSalary> _employeeSalaryRepository;
        private readonly IRepository<Section> _sectionRepository;
        private readonly IRepository<EarningTypes> _earningTypesRepository;
        private readonly IRepository<DeductionTypes> _deductionTypesRepository;
        private readonly IRepository<EmployeeType> _employeeTypeRepository;





        public SalarySheetListingAppService(IRepository<SalarySheet> salarySheetRepository, IRepository<Employees> employeesRepository,
            IRepository<Designations> designationRepository, IRepository<Department> departmentRepository,
            IRepository<EmployeeSalary> employeeSalaryRepository, IRepository<Section> sectionRepository, IRepository<EarningTypes> earningTypesRepository,
            IRepository<EmployeeAdvances> empAdvancesRepository,
            IRepository<DeductionTypes> deductionTypesRepository,
            IRepository<EmployeeType> employeeTypeRepository
            )
        {
            _salarySheetRepository = salarySheetRepository;
            _employeesRepository = employeesRepository;
            _designationRepository = designationRepository;
            _departmentRepository = departmentRepository;
            _employeeSalaryRepository = employeeSalaryRepository;
            _sectionRepository = sectionRepository;
            _earningTypesRepository = earningTypesRepository;
            _deductionTypesRepository = deductionTypesRepository;
            _employeeTypeRepository = employeeTypeRepository;


        }

        public List<SalarySheetListingDto> GetData(int? TenantId, string fromEmpID, string toEmpID, string fromDeptID, string toDeptID,
            string fromSecID, string toSecID, string fromSalary, string toSalary, short SalaryMonth, short SalaryYear, string fromLocId, string toLocId, string Emptype)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }

            var salarySheetData = _salarySheetRepository.GetAll().Where(e => e.SalaryMonth == SalaryMonth && e.SalaryYear == SalaryYear && e.TenantId == TenantId);
            IEnumerable<Employees> employeesData;


            employeesData = _employeesRepository.GetAll().Where(o => o.TenantId == TenantId &&
      o.EmployeeID >= Convert.ToInt32(fromEmpID) && o.EmployeeID <= Convert.ToInt32(toEmpID) &&
      o.DeptID >= Convert.ToInt32(fromDeptID) && o.DeptID <= Convert.ToInt32(toDeptID) &&
      o.SecID >= Convert.ToInt32(fromSecID) && o.SecID <= Convert.ToInt32(toSecID) &&
      o.LocID >= Convert.ToInt32(fromLocId) && o.LocID <= Convert.ToInt32(toLocId)).WhereIf(Emptype != "0", o => o.TypeID == Convert.ToInt32(Emptype)).ToList();




            var employeeTypeData = _employeeTypeRepository.GetAll().Where(o => o.TenantId == TenantId);
            var designationData = _designationRepository.GetAll().Where(o => o.TenantId == TenantId);
            //var employeeSalaryData = _employeeSalaryRepository.GetAll().Where(o => o.TenantId == TenantId);
            var departmentData = _departmentRepository.GetAll().Where(o => o.TenantId == TenantId).OrderBy(x => x.SortId);
            var sectionData = _sectionRepository.GetAll().Where(o => o.TenantId == TenantId);
            var earningTypes = _earningTypesRepository.GetAll().Where(o => o.TenantId == TenantId);
            var deductionTypes = _deductionTypesRepository.GetAll().Where(o => o.TenantId == TenantId);



            var data = from a in salarySheetData
                       join b in employeesData
                       on a.EmployeeID equals b.EmployeeID
                       join c in designationData
                       on b.DesignationID equals c.DesignationID
                       join d in employeeTypeData
                       on b.TypeID equals d.TypeID
                       join e in departmentData
                       on b.DeptID equals e.DeptID
                       join f in sectionData
                       on b.SecID equals f.SecID
                       orderby a.EmployeeID
                       where a.work_days != 0
                       select new SalarySheetListingDto()
                       {
                           EmployeeID = a.EmployeeID.ToString(),
                           EmployeeName = b.EmployeeName,
                           DeptName = e.DeptName.ToUpper() ?? "",
                           Section = f.SecName.ToUpper() ?? "",
                           date_of_joining = b.date_of_joining == null ? b.date_of_joining.ToString() : b.date_of_joining.Value.ToString("dd/MM/yyyy"),
                           date_of_leaving = b.date_of_leaving == null ? b.date_of_leaving.ToString() : b.date_of_leaving.Value.ToString("dd/MM/yyyy"),
                           work_days = a.work_days.Value.ToString("N0") ?? "0",
                           gross_salary = a.gross_salary != null ? a.gross_salary.Value.ToString("N0") : "0",
                           basic_earned = a.basic_earned != null ? a.basic_earned.Value.ToString("N0") : "0",
                           arrears = a.arrears != null ? a.arrears.Value.ToString("N0") : "0",
                           ot_amount = a.ot_amount != null ? a.ot_amount.Value.ToString("N0") : "0",
                           Deduction1 = a.Deduction1 != null ? a.Deduction1.Value.ToString("N0") : "0",
                           Deduction2 = a.Deduction2 != null ? a.Deduction2.Value.ToString("N0") : "0",
                           Deduction3 = a.Deduction3 != null ? a.Deduction3.Value.ToString("N0") : "0",
                           Deduction4 = a.Deduction4 != null ? a.Deduction4.Value.ToString("N0") : "0",
                           Deduction5 = a.Deduction5 != null ? a.Deduction5.Value.ToString("N0") : "0",
                           tax_amount = a.tax_amount != null ? a.tax_amount.Value.ToString("N0") : "0",
                           total_earnings = a.total_earnings != null ? a.total_earnings.Value.ToString("N0") : "0",
                           payment_mode = (b.payment_mode == "0") ? "Cash" : (b.payment_mode == "1") ? "Bank" : (b.payment_mode == "2") ? "Cheque" : "",
                           Income1 = a.Income1 != null ? a.Income1.Value.ToString("N0") : "0",
                           Income2 = a.Income2 != null ? a.Income2.Value.ToString("N0") : "0",
                           Income3 = a.Income3 != null ? a.Income3.Value.ToString("N0") : "0",
                           Income4 = a.Income4 != null ? a.Income4.Value.ToString("N0") : "0",
                           Income5 = a.Income5 != null ? a.Income5.Value.ToString("N0") : "0",
                           EarningTypeName1 = earningTypes.FirstOrDefault(x => x.TypeID == 1).TypeDesc,
                           EarningTypeName2 = earningTypes.FirstOrDefault(x => x.TypeID == 2).TypeDesc,
                           EarningTypeName3 = earningTypes.FirstOrDefault(x => x.TypeID == 3).TypeDesc,
                           EarningTypeName4 = earningTypes.FirstOrDefault(x => x.TypeID == 4).TypeDesc,
                           EarningTypeName5 = earningTypes.FirstOrDefault(x => x.TypeID == 5).TypeDesc,
                           DeductionTypeName1 = deductionTypes.FirstOrDefault(x => x.TypeID == 1).TypeDesc,
                           DeductionTypeName2 = deductionTypes.FirstOrDefault(x => x.TypeID == 2).TypeDesc,
                           DeductionTypeName3 = deductionTypes.FirstOrDefault(x => x.TypeID == 3).TypeDesc,
                           DeductionTypeName4 = deductionTypes.FirstOrDefault(x => x.TypeID == 4).TypeDesc,
                           DeductionTypeName5 = deductionTypes.FirstOrDefault(x => x.TypeID == 5).TypeDesc,
                           //FatherName = b.FatherName,
                           designation = c.Designation,
                           SortId = e.SortId ?? 0,
                           TypeID = b.TypeID,
                           employee_type= Emptype=="0"?"All Employees": d.EmpType,

                           //ot_hrs = a.ot_hrs.ToString(),
                           advance = a.advance != null ? a.advance.Value.ToString("N0") : "0",
                           loan = a.loan != null ? a.loan.Value.ToString("N0") : "0",
                           eobi_amount = a.eobi_amount.ToString(),
                           //Bank_Amount = d.Bank_Amount.ToString(),
                           //MonthYear = a.SalaryDate.ToString("MMMM yyyy"),
                           //Deduction5 = a.Deduction5.ToString(),

                       };

            data = data
             .GroupBy(e => e.EmployeeID)
             .Select(a => a.OrderByDescending(g => g.gross_salary).First());

            return data.OrderBy(o => int.Parse(o.EmployeeID)).ToList();
        }

    }
}
