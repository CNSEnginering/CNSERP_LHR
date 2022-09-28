using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Domain.Repositories;
using ERP.PayRoll.Department;
using ERP.PayRoll.Designation;
using ERP.PayRoll.EmployeeEarnings;
using ERP.PayRoll.Employees;
using ERP.Reports.PayRoll.Dtos;
using Abp.Authorization;
using ERP.Authorization;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ERP.Reports.PayRoll
{
    [AbpAuthorize(AppPermissions.PayRollReports_EmployeeReports)]
    public class TaxCertificateAppService : ERPReportAppServiceBase, ITaxCertificateAppService
    {
        private readonly IRepository<EmployeeEarnings> _employeeEarningsRepository;
        private readonly IRepository<Employees> _employeesRepository;



        public TaxCertificateAppService(IRepository<EmployeeEarnings> employeeEarningsRepository, IRepository<Employees> employeesRepository)
        {
            _employeeEarningsRepository = employeeEarningsRepository;
            _employeesRepository = employeesRepository;
        }

        public List<TaxCertificate> GetData(int? TenantId, string fromEmpID, short SalaryYear, short SalaryMonth, short toSalaryYear, short toSalaryMonth)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;
            }

            SqlCommand cmd;
            string str = ConfigurationManager.AppSettings["ConnectionStringHRM"];
            List<TaxCertificate> taxCertificateDtoList = new List<TaxCertificate>();
            using (SqlConnection cn = new SqlConnection(str))
            {
                cmd = new SqlCommand("sp_TaxCertificate", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TenantId", AbpSession.TenantId);
                cmd.Parameters.AddWithValue("@EmployeeId", fromEmpID);
                cmd.Parameters.AddWithValue("@FromMonth", SalaryMonth);
                cmd.Parameters.AddWithValue("@FromYear", SalaryYear);
                cmd.Parameters.AddWithValue("@ToMonth", toSalaryMonth);
                cmd.Parameters.AddWithValue("@ToYear", toSalaryYear);
                cn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        TaxCertificate taxCertificate = new TaxCertificate()
                        {
                            EmployeeID = rdr["EmployeeID"] is DBNull ? 0 : Convert.ToInt32(rdr["EmployeeID"]),
                            EmployeeName = rdr["EmployeeName"] is DBNull ? "" : rdr["EmployeeName"].ToString(),
                            Address = rdr["Address"] is DBNull ? "" : rdr["Address"].ToString(),
                            CNIC = rdr["Cnic"] is DBNull ? "" : rdr["Cnic"].ToString(),
                            BasicEarned = rdr["basic_earned"] is DBNull ? 0 : Convert.ToDecimal(rdr["basic_earned"].ToString()),
                            TaxAmount = rdr["tax_amount"] is DBNull ? 0 : Convert.ToDecimal(rdr["tax_amount"]),
                            DepositeDate = rdr["CPRDate"] is DBNull ? new DateTime() : Convert.ToDateTime(rdr["CPRDate"]),
                            CPRNumber = rdr["CPRNumber"] is DBNull ? "" : rdr["CPRNumber"].ToString(),
                            SalaryMonth = rdr["SalaryMonth"] is DBNull ? 0 : Convert.ToInt32(rdr["SalaryMonth"]),
                            SalaryYear = rdr["SalaryYear"] is DBNull ? 0 : Convert.ToInt32(rdr["SalaryYear"]),
                            StartDate = rdr["startDate"] is DBNull ? new DateTime() : Convert.ToDateTime(rdr["startDate"]),
                            EndDate = rdr["EndDate"] is DBNull ? new DateTime() : Convert.ToDateTime(rdr["EndDate"]),
                            MedicalAllowance = rdr["basic_earned"] is DBNull ? 0 : (Convert.ToDecimal(rdr["basic_earned"].ToString()) / 110) * 10,
                            NetTaxAbleSalary = rdr["basic_earned"] is DBNull ? 0 : ((Convert.ToDecimal(rdr["basic_earned"].ToString()))-((Convert.ToDecimal(rdr["basic_earned"].ToString()) / 110) * 10)),
                        };
                        taxCertificateDtoList.Add(taxCertificate);
                    }
                }
                cn.Close();
            }
            return taxCertificateDtoList;

        }

    }
}
