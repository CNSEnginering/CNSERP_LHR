using ERP.PayRoll.EmployeeLeaves;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ERP.Reports.PayRoll
{
    public class EmployeeLeavesAppService : ERPAppServiceBase
    {
        public List<EmployeeLeavesLedger> GetEmployeeLeaves(int salaryYear, int frmEmpId, int toEmpId)
        {

            string str = ConfigurationManager.AppSettings["ConnectionStringHRM"];
            var result = new List<EmployeeLeavesLedger>();
            using (SqlConnection cn = new SqlConnection(str))
            {

                using (SqlCommand cmd = new SqlCommand("SP_Emp_Leaves_Ledger", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@frmEmpId", frmEmpId);
                    cmd.Parameters.AddWithValue("@toEmpId", toEmpId);
                    cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                    cmd.Parameters.AddWithValue("@salYear", salaryYear);
                    cn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {


                        while (dataReader.Read())
                        {

                            result.Add(new EmployeeLeavesLedger
                            {
                                EmployeeId = dataReader["EmployeeId"] is DBNull ? 0 : Convert.ToInt32(dataReader["EmployeeId"]),
                                EmployeeName = dataReader["EmployeeName"] is DBNull ? "" : dataReader["EmployeeName"].ToString(),
                                FatherName = dataReader["FatherName"] is DBNull ? "" : dataReader["FatherName"].ToString(),
                                DeptName = dataReader["DeptName"] is DBNull ? "" : dataReader["DeptName"].ToString(),
                                SecName = dataReader["SecName"] is DBNull ? "" : dataReader["SecName"].ToString(),
                                EmpType = dataReader["EmpType"] is DBNull ? "" : dataReader["EmpType"].ToString(),
                                Designation = dataReader["Designation"] is DBNull ? "" : dataReader["Designation"].ToString(),
                                SalaryYear = dataReader["SalaryYear"] is DBNull ? 0 : Convert.ToInt32(dataReader["SalaryYear"]),
                                Annual = dataReader["Annual"] is DBNull ? 0 : Convert.ToInt32(dataReader["Annual"]),
                                Casual = dataReader["Casual"] is DBNull ? 0 : Convert.ToInt32(dataReader["Casual"]),
                                Sick = dataReader["Sick"] is DBNull ? 0 : Convert.ToInt32(dataReader["Sick"]),
                                AnnualTotal = dataReader["AnnualTotal"] is DBNull ? 0 : Convert.ToInt32(dataReader["AnnualTotal"]),
                                CasualTotal = dataReader["CasualTotal"] is DBNull ? 0 : Convert.ToInt32(dataReader["CasualTotal"]),
                                SickTotal = dataReader["SickTotal"] is DBNull ? 0 : Convert.ToInt32(dataReader["SickTotal"]),
                                Date = dataReader["StartDate"] is DBNull ? Convert.ToDateTime("1900/01/01") : Convert.ToDateTime(dataReader["StartDate"]),
                                CPLTotal = dataReader["CPLTotal"] is DBNull ? 0 : Convert.ToInt32(dataReader["CPLTotal"]),
                                Leaves = dataReader["Leaves"] is DBNull ? 0 : Convert.ToInt32(dataReader["Leaves"])
                            });
                        }
                    }
                }
                // cn.Close();
            }
            return result;

        }

    }

    public class EmployeeLeavesLedger
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string FatherName { get; set; }
        public string DeptName { get; set; }
        public string SecName { get; set; }
        public string EmpType { get; set; }
        public string Designation { get; set; }
        public int SalaryYear { get; set; }
        public int Leaves { get; set; }
        public int CasualTotal { get; set; }
        public int SickTotal { get; set; }
        public int CPLTotal { get; set; }
        public int AnnualTotal { get; set; }
        public DateTime Date { get; set; }
        public int Casual { get; set; }
        public int Sick { get; set; }
        public int Annual { get; set; }
    }
}
