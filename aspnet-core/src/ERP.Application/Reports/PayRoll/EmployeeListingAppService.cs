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
using ERP.PayRoll.Section;
using ERP.PayRoll.Employees;
using ERP.PayRoll.EmployeeSalary;
using ERP.PayRoll.SalarySheet;
using Abp.Authorization;
using ERP.Authorization;
using ERP.PayRoll.Allowances;
using ERP.PayRoll.Location;
using ERP.PayRoll.AllowanceSetup;
using ERP.PayRoll.StopSalary;
using ERP.PayRoll;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ERP.Reports.PayRoll
{
    [AbpAuthorize(AppPermissions.PayRollReports_EmployeeReports)]
    public class EmployeeListingAppService : ERPReportAppServiceBase, IEmployeeListingAppService
    {
        private readonly IRepository<EmployeeSalary> _employeeSalaryRepository;
        private readonly IRepository<Employees> _employeeRepository;
        private readonly IRepository<Designations> _designationRepository;
        private readonly IRepository<Department> _deparmentRepository;
        private readonly IRepository<Section> _sectionRepository;
        private readonly IRepository<Locations> _locationRepository;
        private readonly IRepository<AllowancesDetail> _AllowanceDRepository;
        private readonly IRepository<Allowances> _AllowanceRepository;
        private readonly IRepository<AllowanceSetup> _allowanceSetupRepository;
        private readonly IRepository<StopSalary> _StopSalaryRepository;

        private readonly IRepository<EmployerBank> _employerBankRepository;
        public EmployeeListingAppService(IRepository<AllowanceSetup> allowanceSetupRepository,
            IRepository<EmployeeSalary> employeeSalaryRepository,
            IRepository<Employees> employeeRepository,
            IRepository<Designations> designationRepository,
            IRepository<Department> deparmentRepository,
            IRepository<Section> sectionRepository,
              IRepository<Locations> locationRepository,
            IRepository<AllowancesDetail> allowanceDRepository, IRepository<Allowances> allowanceRepository, IRepository<StopSalary> StopSalaryRepository
           , IRepository<EmployerBank> employerBankRepository)

        {
            _employeeSalaryRepository = employeeSalaryRepository;
            _employeeRepository = employeeRepository;
            _designationRepository = designationRepository;
            _deparmentRepository = deparmentRepository;
            _sectionRepository = sectionRepository;
            _locationRepository = locationRepository;
            _AllowanceDRepository = allowanceDRepository;
            _AllowanceRepository = allowanceRepository;
            _allowanceSetupRepository = allowanceSetupRepository;
            _StopSalaryRepository = StopSalaryRepository;
            _employerBankRepository = employerBankRepository;
        }
        public List<EmployeeListingDto> GetData(int? TenantId, string fromEmpID, string toEmpID, string fromDeptID, string toDeptID,
            string fromSecID, string toSecID, string fromSalary, string toSalary, bool isActive, string fromlocid, string tolocid, string emptype)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;
            }
            var fLocID = Convert.ToInt32(fromlocid);
            var TLocID = Convert.ToInt32(tolocid);
            var empty = Convert.ToInt32(emptype);
            var employee = _employeeRepository.GetAll().Where(o => o.TenantId == TenantId && o.Active == isActive &&
            o.EmployeeID >= Convert.ToInt32(fromEmpID) && o.EmployeeID <= Convert.ToInt32(toEmpID) &&
            o.DeptID >= Convert.ToInt32(fromDeptID) && o.DeptID <= Convert.ToInt32(toDeptID) &&
            o.SecID >= Convert.ToInt32(fromSecID) && o.SecID <= Convert.ToInt32(toSecID));

            var department = _deparmentRepository.GetAll().Where(o => o.TenantId == TenantId);
            var designation = _designationRepository.GetAll().Where(o => o.TenantId == TenantId);
            var section = _sectionRepository.GetAll().Where(o => o.TenantId == TenantId);
            var location = _locationRepository.GetAll().Where(o => o.TenantId == TenantId);
            var employeeSalary = _employeeSalaryRepository.GetAll().Where(o => o.TenantId == TenantId &&
            o.Gross_Salary >= Convert.ToInt32(fromSalary) && o.Gross_Salary <= Convert.ToInt32(toSalary));

            var data = from a in employee
                       join b in department on a.DeptID equals b.DeptID
                       join c in designation on a.DesignationID equals c.DesignationID
                       join d in employeeSalary on a.EmployeeID equals d.EmployeeID into emp
                       join e in section on a.SecID equals e.SecID
                       join f in location on a.LocID equals f.LocID
                       from employees in emp.DefaultIfEmpty()
                       select new EmployeeListingDto()
                       {

                           EmpID = a.EmployeeID.ToString(),
                           EmpName = a.EmployeeName,
                           FatherName = a.FatherName,
                           Department = b.DeptName.ToUpper() ?? "",
                           Section = e.SecName.ToUpper() ?? "",
                           Designation = c.Designation,
                           LocationName = f.Location,
                           Joining = a.date_of_joining == null ? a.date_of_joining.ToString() : a.date_of_joining.Value.ToString("dd/MM/yyyy"),
                           Leaving = a.date_of_leaving == null ? a.date_of_leaving.ToString() : a.date_of_leaving.Value.ToString("dd/MM/yyyy"),
                           GrossSalary = employees.Gross_Salary.ToString(),
                           LocID = a.LocID,
                           Emptype = a.TypeID
                       };
            if (empty > 0)
            {
                data = data.Where(x => x.LocID >= fLocID && x.LocID <= TLocID && x.Emptype == empty)
           .GroupBy(e => e.EmpID)
           .Select(a => a.OrderByDescending(g => g.GrossSalary).First());
            }
            else
            {
                data = data.Where(x => x.LocID >= fLocID && x.LocID <= TLocID)
           .GroupBy(e => e.EmpID)
           .Select(a => a.OrderByDescending(g => g.GrossSalary).First());
            }


            //var pin = data.OrderByDescending(p => p.GrossSalary).FirstOrDefault();

            return data.ToList();
        }
        public static string NumberToText(int n)
        {
            if (n < 0)
                return "Minus " + NumberToText(-n);
            else if (n == 0)
                return "";
            else if (n <= 19)
                return new string[] {"One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight",
         "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
         "Seventeen", "Eighteen", "Nineteen"}[n - 1] + " ";
            else if (n <= 99)
                return new string[] {"Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy",
         "Eighty", "Ninety"}[n / 10 - 2] + " " + NumberToText(n % 10);
            else if (n <= 199)
                return "One Hundred " + NumberToText(n % 100);
            else if (n <= 999)
                return NumberToText(n / 100) + "Hundreds " + NumberToText(n % 100);
            else if (n <= 1999)
                return "One Thousand " + NumberToText(n % 1000);
            else if (n <= 999999)
                return NumberToText(n / 1000) + "Thousands " + NumberToText(n % 1000);
            else if (n <= 1999999)
                return "One Million " + NumberToText(n % 1000000);
            else if (n <= 999999999)
                return NumberToText(n / 1000000) + "Millions " + NumberToText(n % 1000000);
            else if (n <= 1999999999)
                return "One Billion " + NumberToText(n % 1000000000);
            else
                return NumberToText(n / 1000000000) + "Billions " + NumberToText(n % 1000000000);
        }
        public List<AllowanceEmployeeListingDto> GetDataForAllowance(int? TenantId, string fromEmpID, string toEmpID, string fromDeptID, string toDeptID, string fromSecID, string toSecID, string allowanceYear, string AllowanceMonth, bool isActive, string fromlocid, string tolocid, string Allowtype, string EmpType, string allowanceBtype)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;
            }
            var fLocID = Convert.ToInt32(fromlocid);
            var TLocID = Convert.ToInt32(tolocid);
            var empty = Convert.ToInt32(EmpType);

            var employee = _employeeRepository.GetAll().Where(o => o.TenantId == TenantId && o.Active == isActive &&
            o.EmployeeID >= Convert.ToInt32(fromEmpID) && o.EmployeeID <= Convert.ToInt32(toEmpID) &&
            o.DeptID >= Convert.ToInt32(fromDeptID) && o.DeptID <= Convert.ToInt32(toDeptID) &&
            o.SecID >= Convert.ToInt32(fromSecID) && o.SecID <= Convert.ToInt32(toSecID));

            var location = _locationRepository.GetAll().Where(o => o.TenantId == TenantId);
            var department = _deparmentRepository.GetAll().Where(o => o.TenantId == TenantId);
            var designation = _designationRepository.GetAll().Where(o => o.TenantId == TenantId);
            var section = _sectionRepository.GetAll().Where(o => o.TenantId == TenantId);
            var employeeHAllowance = _AllowanceRepository.GetAll().Where(o => o.TenantId == TenantId &&
            o.DocYear == Convert.ToInt32(allowanceYear) && o.DocMonth == Convert.ToInt32(AllowanceMonth)).FirstOrDefault();
            
            var employeeDAllowance = _AllowanceDRepository.GetAll().Where(o => o.TenantId == TenantId && o.EmployeeID >= Convert.ToInt32(fromEmpID)
            && o.EmployeeID <= Convert.ToInt32(toEmpID) && o.DetID == employeeHAllowance.Id
            );
            var SetupData = _allowanceSetupRepository.GetAll().Where(x => x.TenantId == TenantId).FirstOrDefault();
            var StopsalaryData = _StopSalaryRepository.GetAll().Where(x => x.TenantId == TenantId && x.TypeID == 3 && x.SalaryMonth == Convert.ToInt32(AllowanceMonth) && x.SalaryYear == Convert.ToInt32(allowanceYear));
            var allowanceStoppedEmps = from e in employee join s in StopsalaryData on e.EmployeeID equals s.EmployeeID select e;
            var employerBank = _employerBankRepository.GetAll().Where(x => x.TenantId == TenantId && x.Active == true);

            if (allowanceBtype == "AllowanceBank")
            {
                var data = //from k in employeeHAllowance
                           from l in employeeDAllowance //on k.Id equals l.DetID
                           join m in employee.Except(allowanceStoppedEmps) on l.EmployeeID equals m.EmployeeID
                           join n in department on m.DeptID equals n.DeptID
                           join o in section on m.SecID equals o.SecID
                           join p in designation on m.DesignationID equals p.DesignationID
                           join f in location on m.LocID equals f.LocID
                           join c in employerBank on m.EBankID equals c.EBankID
                           select new AllowanceEmployeeListingDto()
                           {

                               ClientAccNo = c.EBranchID + "-" + c.EBankAccNumber,
                               Date = DateTime.Now.ToString("dd MMMM yyyy"),

                               SalaryAcc = m.account_no,//Substring(m.account_no.IndexOf("-") , m.account_no.Length),
                               AccTitle = m.EmployeeName,
                               Amount = l.Amount.ToString(),
                               OurBranch = c.EBranchID,
                               TheirBranch = m.account_no,// m.account_no.Substring(0, m.account_no.LastIndexOf("-")),
                               EmpID = m.EmployeeID.ToString(),
                               EmpName = m.EmployeeName,
                               FatherName = m.FatherName,
                               Department = n.DeptName.ToUpper() ?? "",
                               Section = o.SecName.ToUpper() ?? "",
                               LocationName = f.Location,
                               Designation = p.Designation,
                               Joining = m.date_of_joining == null ? m.date_of_joining.ToString() : m.date_of_joining.Value.ToString("dd/MM/yyyy"),
                               Leaving = m.date_of_leaving == null ? m.date_of_leaving.ToString() : m.date_of_leaving.Value.ToString("dd/MM/yyyy"),
                               // GrossSalary = employees.Gross_Salary.ToString(),
                               LocID = m.LocID,
                               Emptype = m.TypeID,
                               allowanceTypeName = Convert.ToInt32(Allowtype) == 1 ? "Car" : "MotorCycle",
                               FixedAllowanceAmt = l.AllowanceAmt,
                               FuelInLiter = l.AllowanceQty,
                               PerMilageRate = l.PerlitrMilg,
                               PerlitrMilage = SetupData.PerLiterMilage,
                               FuelDate = SetupData.AudtDate,
                               FuelRate = SetupData.FuelRate,
                               RepairRate = l.RepairRate,
                               TotalAmount = l.Amount,
                               Milage = l.Milage,
                               payment_mode = m.payment_mode,
                               allowanceTypeID = m.AllowanceType.ToString()

                           };
                if (empty > 0)
                {
                    data = data.Where(x => x.Emptype == empty);

                }
                data = data.Where(x => x.payment_mode == "1");



                // wordData.FirstOrDefault().TotalAmtWord = null;
                //var pin = data.OrderByDescending(p => p.GrossSalary).FirstOrDefault();

                return data == null ? null : data.ToList();
            }
            else
            if (allowanceBtype == "Allowance")
            {
                var data1 = //from k in employeeHAllowance
                            from l in employeeDAllowance //on k.Id equals l.DetID
                            join m in employee on l.EmployeeID equals m.EmployeeID
                            join n in department on m.DeptID equals n.DeptID
                            join o in section on m.SecID equals o.SecID
                            join p in designation on m.DesignationID equals p.DesignationID
                            join f in location on m.LocID equals f.LocID
                            select new AllowanceEmployeeListingDto()
                            {

                                EmpID = m.EmployeeID.ToString(),
                                EmpName = m.EmployeeName,
                                FatherName = m.FatherName,
                                Department = n.DeptName.ToUpper() ?? "",
                                Section = o.SecName.ToUpper() ?? "",
                                LocationName = f.Location,
                                Designation = p.Designation,
                                Joining = m.date_of_joining == null ? m.date_of_joining.ToString() : m.date_of_joining.Value.ToString("dd/MM/yyyy"),
                                Leaving = m.date_of_leaving == null ? m.date_of_leaving.ToString() : m.date_of_leaving.Value.ToString("dd/MM/yyyy"),
                                // GrossSalary = employees.Gross_Salary.ToString(),
                                LocID = m.LocID,
                                Emptype = m.TypeID,
                                allowanceTypeName = Convert.ToInt32(Allowtype) == 1 ? "Car" : "MotorCycle",
                                FixedAllowanceAmt = l.AllowanceAmt,
                                FuelInLiter = l.AllowanceQty,
                                PerMilageRate = l.PerlitrMilg,
                                PerlitrMilage = SetupData.PerLiterMilage,
                                FuelDate = SetupData.FuelDate,
                                FuelRate = SetupData.FuelRate,
                                RepairRate = l.RepairRate,
                                TotalAmount = l.Amount,
                                Milage = l.Milage,
                                payment_mode = m.payment_mode,
                                allowanceTypeID = m.AllowanceType.ToString()
                            };
                if (empty > 0)
                {
                    data1 = data1.Where(x => x.Emptype == empty);

                }
                if (Allowtype == "1")
                {
                    data1 = data1.Where(o => o.FuelInLiter > 0 && o.allowanceTypeID == Allowtype);
                }
                else
                {
                    data1 = data1.Where(o => o.Milage > 0 && o.allowanceTypeID == Allowtype);
                }
                //data1 = data1.Where(o => o.FuelInLiter > 0);
                var wordData = data1.ToList();
                //wordData.FirstOrDefault().TotalAmtWord = "";
                //var pin = data.OrderByDescending(p => p.GrossSalary).FirstOrDefault();

                return data1.ToList();
            }
            if (allowanceBtype == "FixedAllowance")
            {
                var data1 = //from k in employeeHAllowance
                            from l in employeeDAllowance //on k.Id equals l.DetID
                            join m in employee on l.EmployeeID equals m.EmployeeID
                            join n in department on m.DeptID equals n.DeptID
                            join o in section on m.SecID equals o.SecID
                            join p in designation on m.DesignationID equals p.DesignationID
                            join f in location on m.LocID equals f.LocID
                            select new AllowanceEmployeeListingDto()
                            {

                                EmpID = m.EmployeeID.ToString(),
                                EmpName = m.EmployeeName,
                                FatherName = m.FatherName,
                                Department = n.DeptName.ToUpper() ?? "",
                                Section = o.SecName.ToUpper() ?? "",
                                LocationName = f.Location,
                                Designation = p.Designation,
                                Joining = m.date_of_joining == null ? m.date_of_joining.ToString() : m.date_of_joining.Value.ToString("dd/MM/yyyy"),
                                Leaving = m.date_of_leaving == null ? m.date_of_leaving.ToString() : m.date_of_leaving.Value.ToString("dd/MM/yyyy"),
                                // GrossSalary = employees.Gross_Salary.ToString(),
                                LocID = m.LocID,
                                Emptype = m.TypeID,
                                allowanceTypeName = Convert.ToInt32(Allowtype) == 1 ? "Car" : "MotorCycle",
                                FixedAllowanceAmt = l.AllowanceAmt,
                                FuelInLiter = l.AllowanceQty,
                                PerMilageRate = l.PerlitrMilg,
                                PerlitrMilage = SetupData.PerLiterMilage,
                                FuelDate = SetupData.FuelDate,
                                FuelRate = SetupData.FuelRate,
                                RepairRate = l.RepairRate,
                                TotalAmount = l.Amount,
                                Milage = l.Milage,
                                payment_mode = m.payment_mode,
                                allowanceTypeID = m.AllowanceType.ToString()
                            };
                if (empty > 0)
                {
                    data1 = data1.Where(x => x.Emptype == empty);

                }
                data1 = data1.Where(o => o.FixedAllowanceAmt > 0 && o.allowanceTypeID == "1");
                var wordData = data1.ToList();
                //wordData.FirstOrDefault().TotalAmtWord = "";
                //var pin = data.OrderByDescending(p => p.GrossSalary).FirstOrDefault();

                return data1.ToList();
            }
            else
            {
                return null;
            }


        }


        public List<SalarySheetSummaryDto> GetDataForAllowanceDisburseMent(int? TenantId, string fromEmpID, string toEmpID, string fromDeptID, string toDeptID,
         string fromSecID, string toSecID, string allowanceYear, string AllowanceMonth, bool isActive, string fromlocid, string tolocid, string Allowtype, string EmpType, string allowanceBtype)
        {


            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }

            var AllowanceHData = _AllowanceRepository.GetAll().Where(e => e.DocMonth == Convert.ToInt32(AllowanceMonth) && e.DocYear == Convert.ToInt32(allowanceYear) && e.TenantId == TenantId).FirstOrDefault();
            var AllowanceDData = _AllowanceDRepository.GetAll().Where(e => e.TenantId == TenantId && e.DetID == AllowanceHData.Id);

            var employee = _employeeRepository.GetAll().Where(o => o.TenantId == TenantId && o.Active == isActive &&
          o.EmployeeID >= Convert.ToInt32(fromEmpID) && o.EmployeeID <= Convert.ToInt32(toEmpID) &&
          o.DeptID >= Convert.ToInt32(fromDeptID) && o.DeptID <= Convert.ToInt32(toDeptID) &&
          o.SecID >= Convert.ToInt32(fromSecID) && o.SecID <= Convert.ToInt32(toSecID));

            var StopsalaryData = _StopSalaryRepository.GetAll().Where(x => x.TenantId == TenantId && x.TypeID == 3 && x.SalaryMonth == Convert.ToInt32(AllowanceMonth) && x.SalaryYear == Convert.ToInt32(allowanceYear));
            var allowanceStoppedEmps = from e in employee
                                       join s in StopsalaryData on e.EmployeeID equals s.EmployeeID
                                       join l in _locationRepository.GetAll().Where(x => x.TenantId == TenantId) on e.LocID equals l.LocID
                                       join c in _designationRepository.GetAll().Where(o => o.TenantId == TenantId)
                                       on e.DesignationID equals c.DesignationID
                                       join d in _deparmentRepository.GetAll().Where(o => o.TenantId == TenantId)
                                       on e.DeptID equals d.DeptID
                                       select e;


            var employeesData = _employeeRepository.GetAll().Where(o => o.TenantId == TenantId);

            var locations = _locationRepository.GetAll().Where(o => o.TenantId == TenantId);

            var data2 = from a in AllowanceDData
                        join b in employee.Except(allowanceStoppedEmps) on a.EmployeeID equals b.EmployeeID
                        join l in locations
                        on b.LocID equals l.LocID
                        join c in _designationRepository.GetAll().Where(o => o.TenantId == TenantId)
                        on b.DesignationID equals c.DesignationID
                        join d in _deparmentRepository.GetAll().Where(o => o.TenantId == TenantId)
                        on b.DeptID equals d.DeptID

                        select new SalarySheetSummaryDto()
                        {
                            EmployeeID = a.EmployeeID.ToString(),
                            EmployeeName = b.EmployeeName,
                            Amount = a.Amount.Value.ToString("N0"),
                            Designation = c.Designation,
                            Department = d.DeptName,
                            SalaryMonth = 11,
                            SalaryYear = 4,

                            //ModOfPay = a.ModOfPay.ToString()
                            ModOfPay = (b.payment_mode == "0") ? "Cash" : (b.payment_mode == "1") ? "Bank Advice"
                            : (b.payment_mode == "2") ? "Cheque" : "",
                            Location = l.Location
                        };
            var allowanceStoppedEmpsfordto = from e in employee
                                       join s in StopsalaryData on e.EmployeeID equals s.EmployeeID
                                       join l in _locationRepository.GetAll().Where(x => x.TenantId == TenantId) on e.LocID equals l.LocID
                                       join c in _designationRepository.GetAll().Where(o => o.TenantId == TenantId)
                                       on e.DesignationID equals c.DesignationID
                                       join d in _deparmentRepository.GetAll().Where(o => o.TenantId == TenantId)
                                       on e.DeptID equals d.DeptID
                                       join al in AllowanceDData on s.EmployeeID equals al.EmployeeID
                                       
                                       select new SalarySheetSummaryDto()
                                       {
                                           EmployeeID=al.EmployeeID.ToString(),
                                           EmployeeName=e.EmployeeName,
                                           Amount=al.Amount.ToString(),
                                           Designation=c.Designation,
                                           Department=d.DeptName,
                                           SalaryMonth=11,
                                           SalaryYear=4,
                                           Location=l.Location,
                                           ModOfPay="Stopped"
                                       };
            var data2ConvertList = data2.ToList();
            SalarySheetSummaryDto dt = new SalarySheetSummaryDto();
            if (allowanceStoppedEmpsfordto != null)
            {
                data2ConvertList.AddRange(allowanceStoppedEmpsfordto);
            }


            return data2ConvertList;
        }

        public List<AllowanceSummaryDto> GetDataForAllowanceSummary(int? TenantId, string fromEmpID, string toEmpID, string fromDeptID, string toDeptID,
         string fromSecID, string toSecID, string allowanceYear, string AllowanceMonth, bool isActive, string fromlocid, string tolocid, string Allowtype, string EmpType, string allowanceBtype)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;
            }
            string str = ConfigurationManager.AppSettings["ConnectionStringHRM"];
            var result = new List<AllowanceSummaryDto>();

            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("Sp_AllowanceSummary", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tenantId", TenantId);
                    cmd.Parameters.AddWithValue("@Month", AllowanceMonth);
                    cmd.Parameters.AddWithValue("@Year", allowanceYear);
                    cmd.Parameters.AddWithValue("@FromEmp", fromEmpID);
                    cmd.Parameters.AddWithValue("@ToEmp", toEmpID);
                    cmd.Parameters.AddWithValue("@FromDept", fromDeptID);
                    cmd.Parameters.AddWithValue("@ToDept", toDeptID);
                    cmd.Parameters.AddWithValue("@FromSec", fromSecID);
                    cmd.Parameters.AddWithValue("@ToSec", toSecID);
                    cmd.Parameters.AddWithValue("@FromLoc", fromlocid);
                    cmd.Parameters.AddWithValue("@ToLoc", tolocid);
                    cmd.Parameters.AddWithValue("@AllownaceType", Allowtype);
                    cmd.Parameters.AddWithValue("@empType", EmpType);
                    cn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {

                        while (dataReader.Read())
                        {
                            var rec = new AllowanceSummaryDto();
                            rec.NetAmount = dataReader["NetAmount"] is DBNull ? 0 : float.Parse(dataReader["NetAmount"].ToString());
                            rec.FuelInLiter = dataReader["FuelInLiter"] is DBNull ? 0 : float.Parse(dataReader["FuelInLiter"].ToString());
                            rec.FixedAllowance = dataReader["FixedAllowance"] is DBNull ? 0 : float.Parse(dataReader["FixedAllowance"].ToString());
                            rec.DeptID = dataReader["DeptID"] is DBNull ? "" : dataReader["DeptID"].ToString();
                            rec.DeptName = dataReader["DeptName"] is DBNull ? "" : dataReader["DeptName"].ToString();
                            rec.SecID = dataReader["SecID"] is DBNull ? "" : dataReader["SecID"].ToString();
                            rec.SecName = dataReader["SecName"] is DBNull ? "" : dataReader["SecName"].ToString();
                            rec.totalEmployee = dataReader["totalEmployee"] is DBNull ? "" : dataReader["totalEmployee"].ToString();
                            rec.TenantId = dataReader["TenantId"] is DBNull ? "" : dataReader["TenantId"].ToString();
                            rec.DocYear = dataReader["DocYear"] is DBNull ? "" : dataReader["DocYear"].ToString();
                            rec.DocMonth = dataReader["DocMonth"] is DBNull ? "" : dataReader["DocMonth"].ToString();
                            rec.LocID = dataReader["LocID"] is DBNull ? "" : dataReader["LocID"].ToString();
                            rec.AllowanceType = dataReader["AllowanceType"] is DBNull ? "" : dataReader["AllowanceType"].ToString();
                            rec.TypeID = dataReader["TypeID"] is DBNull ? "" : dataReader["TypeID"].ToString();
                            result.Add(rec);
                        }

                    }
                    cn.Close();
                }
            }
            return result;
        }

    }
}
