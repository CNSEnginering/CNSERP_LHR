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
using ERP.PayRoll.EmployeeSalary;
using ERP.PayRoll.SalarySheet;
using ERP.PayRoll.Section;
using ERP.PayRoll.Religion;
using ERP.PayRoll.Grades;
using ERP.PayRoll.Education;
using ERP.PayRoll.EmployeeType;
using Abp.Authorization;
using ERP.Authorization;

namespace ERP.Reports.PayRoll
{
    [AbpAuthorize(AppPermissions.PayRollReports_EmployeeReports)]
    public class EmployeeCardListingAppService : ERPReportAppServiceBase, IEmployeeCardListingAppService
    {
        private readonly IRepository<Employees> _employeeRepository;
        private readonly IRepository<Designations> _designationRepository;
        private readonly IRepository<Department> _deparmentRepository;
        private readonly IRepository<Section> _sectionRepository;
        private readonly IRepository<Religions> _religionRepository;
        private readonly IRepository<Grade> _gradeRepository;
        private readonly IRepository<Shift> _shiftRepository;
        private readonly IRepository<Education> _educationRepository;
        private readonly IRepository<EmployeeType> _employeeTypeRepository;

        public EmployeeCardListingAppService(
            IRepository<SalarySheet> salarySheetRepository,
            IRepository<Employees> employeeRepository,
            IRepository<Designations> designationRepository,
            IRepository<Department> deparmentRepository,
            IRepository<Section> sectionRepository,
            IRepository<Religions> religionRepository,
            IRepository<Grade> gradeRepository,
            IRepository<Shift> shiftRepository,
            IRepository<Education> educationRepository,
            IRepository<EmployeeType> employeeTypeRepository
            )
        {
            _employeeRepository = employeeRepository;
            _designationRepository = designationRepository;
            _deparmentRepository = deparmentRepository;
            _sectionRepository = sectionRepository;
            _religionRepository = religionRepository;
            _gradeRepository = gradeRepository;
            _shiftRepository = shiftRepository;
            _educationRepository = educationRepository;
            _employeeTypeRepository = employeeTypeRepository;
        }
        public List<EmployeeCardListingDto> GetData(int? TenantId, string fromEmpID, string toEmpID, string fromDeptID, string toDeptID,
            string fromSecID, string toSecID, string fromSalary, string toSalary)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }

            var employee = _employeeRepository.GetAll().Where(o => o.TenantId == TenantId &&
            o.EmployeeID >= Convert.ToInt32(fromEmpID) && o.EmployeeID <= Convert.ToInt32(toEmpID) &&
            o.DeptID >= Convert.ToInt32(fromDeptID) && o.DeptID <= Convert.ToInt32(toDeptID) &&
            o.SecID >= Convert.ToInt32(fromSecID) && o.SecID <= Convert.ToInt32(toSecID));

            var department = _deparmentRepository.GetAll().Where(o => o.TenantId == TenantId);
            var designation = _designationRepository.GetAll().Where(o => o.TenantId == TenantId);
            var section = _sectionRepository.GetAll().Where(o => o.TenantId == TenantId);
            var religion = _religionRepository.GetAll().Where(o => o.TenantId == TenantId);
            var grade = _gradeRepository.GetAll().Where(o => o.TenantId == TenantId);
            var shift = _shiftRepository.GetAll().Where(o => o.TenantId == TenantId);
            var education = _educationRepository.GetAll().Where(o => o.TenantId == TenantId);
            var employeeType = _employeeTypeRepository.GetAll().Where(o => o.TenantId == TenantId);

            var data = from a in employee orderby a.EmployeeID ascending
                       join b in department on a.DeptID equals b.DeptID
                       join c in designation on a.DesignationID equals c.DesignationID
                       join d in section on a.SecID equals d.SecID
                       join e in religion on a.ReligionID equals e.ReligionID
                       join f in shift on a.ShiftID equals f.ShiftID
                       join g in education on a.EdID equals g.EdID
                       join h in employeeType on a.TypeID equals h.TypeID
                       select new EmployeeCardListingDto()
                       {

                           EmployeeID = a.EmployeeID.ToString(),
                           EmployeeName = a.EmployeeName,
                           FatherName = a.FatherName,
                           Department = b.DeptName,
                           Designation = c.Designation,
                           Section = d.SecName,
                           Religion = e.Religion,
                           Shift = f.ShiftName,
                           Education = g.Eduction,
                           EmpType = h.EmpType,
                           Gender = a.Gender == "M" ? "Male" : "Female",
                           Date_of_Birth = a.date_of_birth == null ? a.date_of_birth.ToString() : a.date_of_birth.Value.ToString("dd/MM/yyyy"),
                           Status = a.Active == true ? "Active" : "InActive",
                           Confirmation = a.Confirmed == true ? "Confirmed" : "Not Confirmed",
                           Academic_qualification = a.academic_qualification,
                           Bank = a.bank_name,
                           AccountNo = a.account_no,
                           Cnic = a.Cnic,
                           BloodGroup = a.BloodGroup,
                           Appointment = a.apointment_date == null ? a.apointment_date.ToString() : a.apointment_date.Value.ToString("dd/MM/yyyy"),
                           Joining = a.date_of_joining == null ? a.date_of_joining.ToString() : a.date_of_joining.Value.ToString("dd/MM/yyyy"),
                           Leaving = a.date_of_leaving == null ? a.date_of_leaving.ToString() : a.date_of_leaving.Value.ToString("dd/MM/yyyy"),
                           First_rest_days = a.first_rest_days==null? a.first_rest_days.ToString(): a.first_rest_days==1 ? "Saturday": "Sunday",
                           Second_rest_days = a.second_rest_days == null ? a.second_rest_days.ToString() : a.second_rest_days==1 ? "Saturday" : "Sunday",
                           //First_rest_days_w2 = a.first_rest_days_w2.ToString(),
                           //Second_rest_days_w2 = a.second_rest_days_w2.ToString(),
                           Reference = a.Reference,
                           Eobi = a.eobi == true ? "Yes" : "No",
                           Wppf = a.wppf == true ? "Yes" : "No",
                           Social_security = a.social_security == true ? "Yes" : "No",
                       };

            return data.ToList();
        }

    }
}
