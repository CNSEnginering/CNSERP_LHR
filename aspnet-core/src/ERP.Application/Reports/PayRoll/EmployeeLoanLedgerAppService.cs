using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ERP.Reports.PayRoll
{

    public class EmployeeLoanLedgerAppService : ERPReportAppServiceBase
    {
        public List<EmployeeLoanLedger> GeEmployeeLoanLedger(int? fromEmpID, int? toEmpId, int? loanTypeId)
        {
            SqlCommand cmd;
            string str = ConfigurationManager.AppSettings["ConnectionStringHRM"];
            List<EmployeeLoanLedger> loanLedgerModelDtoList = new List<EmployeeLoanLedger>();
            using (SqlConnection cn = new SqlConnection(str))
            {


                cmd = new SqlCommand("SP_EmpLoans", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@loanTypeId", loanTypeId);
                cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                cmd.Parameters.AddWithValue("@frmEmpId", fromEmpID);
                cmd.Parameters.AddWithValue("@toEmpId", toEmpId);
                cn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        EmployeeLoanLedger employeeLoanLedger = new EmployeeLoanLedger()
                        {
                            EmployeeId = rdr["EmployeeId"] is DBNull ? 0 : Convert.ToInt32(rdr["EmployeeId"]),
                            EmployeeName = rdr["EmployeeName"] is DBNull ? "" : rdr["EmployeeName"].ToString(),
                            FatherName = rdr["FatherName"] is DBNull ? "" : rdr["FatherName"].ToString(),
                            DeptName = rdr["DeptName"] is DBNull ? "" : rdr["DeptName"].ToString(),
                            designation = rdr["designation"] is DBNull ? "" : rdr["designation"].ToString(),
                            LoanTypeName = rdr["LoanTypeName"] is DBNull ? "" : rdr["LoanTypeName"].ToString(),
                            Amount = rdr["Amount"] is DBNull ? 0 : Convert.ToDouble(rdr["Amount"]),
                            LoanDate = rdr["LoanDate"] is DBNull ? new DateTime() : Convert.ToDateTime(rdr["LoanDate"]),
                            InsAmt = rdr["InsAmt"] is DBNull ? 0 : Convert.ToDouble(rdr["InsAmt"]),
                            NOI = rdr["NOI"] is DBNull ? 0 : Convert.ToInt32(rdr["NOI"]),
                            DeptID = rdr["DeptID"] is DBNull ? 0 : Convert.ToInt32(rdr["DeptID"]),
                            LoanID = rdr["LoanID"] is DBNull ? 0 : Convert.ToInt32(rdr["LoanID"]),
                            LoanMonth = rdr["LoanMonth"] is DBNull ? 0 : Convert.ToInt32(rdr["LoanMonth"]),
                            LoanYear = rdr["LoanYear"] is DBNull ? 0 : Convert.ToInt32(rdr["LoanYear"]),
                            AdjustSalaryYear = rdr["AdjustSalaryYear"] is DBNull ? 0 : Convert.ToInt32(rdr["AdjustSalaryYear"]),
                            AdjustSalaryMonth = rdr["AdjustSalaryMonth"] is DBNull ? 0 : Convert.ToInt32(rdr["AdjustSalaryMonth"]),
                        };

                        loanLedgerModelDtoList.Add(employeeLoanLedger);

                    }
                }
            }
            return loanLedgerModelDtoList;

        }
    }
}

public class EmployeeLoanLedger
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public string FatherName { get; set; }
    public string DeptName { get; set; }
    public string designation { get; set; }
    public string LoanTypeName { get; set; }
    public double Amount { get; set; }
    public DateTime LoanDate { get; set; }
    public double InsAmt { get; set; }
    public int NOI { get; set; }
    public int DeptID { get; set; }
    public int LoanID { get; set; }
    public int LoanMonth { get; set; }
    public int LoanYear { get; set; }
    public int AdjustSalaryMonth { get; set; }
    public int AdjustSalaryYear { get; set; }
    public double AdjustAmt { get; set; }

}


