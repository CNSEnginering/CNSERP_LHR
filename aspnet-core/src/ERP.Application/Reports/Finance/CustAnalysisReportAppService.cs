using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using static ERP.Reports.Finance.GeneralLedgerReportAppService;

namespace ERP.Reports.Finance
{
    public class CustAnalysisReportAppService : ERPReportAppServiceBase
    {
        public List<GeneralLedgerDto> GetCustAnalysisReport(DateTime fromDate, DateTime toDate, string status, int slType, string fromAcc, string toAcc, string fromSubAcc, string toSubAcc ,int? curRate, int loc)
        {
            var tenantId = AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<GeneralLedgerDto> generalLedgerDtoList = new List<GeneralLedgerDto>();

            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = (status == "Both" ? new SqlCommand("SP_Cust_Analysis_Rpt_Both", cn) : new SqlCommand("SP_Cust_Analysis_Rpt", cn)))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromDate", fromDate.Date);
                    cmd.Parameters.AddWithValue("@toDate", toDate.Date);
                    cmd.Parameters.AddWithValue("@tenantId", tenantId);
                    cmd.Parameters.AddWithValue("@slType", slType);
                    cmd.Parameters.AddWithValue("@LedgerId", slType);
                    cmd.Parameters.AddWithValue("@fromAcc", fromAcc);
                    cmd.Parameters.AddWithValue("@toAcc", toAcc);
                    cmd.Parameters.AddWithValue("@fromSubAcc", fromSubAcc);
                    cmd.Parameters.AddWithValue("@toSubAcc", toSubAcc);
                    if (loc == 0)
                    {
                        cmd.Parameters.AddWithValue("@fromLoc", 0);
                        cmd.Parameters.AddWithValue("@toLoc", 9999);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@fromLoc", loc);
                        cmd.Parameters.AddWithValue("@toLoc", loc);
                    }

                    if (status == "Approved")
                    {
                        cmd.Parameters.AddWithValue("@Approved", 1);
                        cmd.Parameters.AddWithValue("@Posted", 0);
                    }
                    else if (status == "Posted")
                    {
                        cmd.Parameters.AddWithValue("@Approved", 1);
                        cmd.Parameters.AddWithValue("@Posted", 1);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Approved", 1);
                        cmd.Parameters.AddWithValue("@Posted", 1);
                    }

                    cn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            GeneralLedgerDto generalLedgerDto = new GeneralLedgerDto()
                            {
                                LocId = Convert.ToInt32(rdr["LocId"].ToString()),
                                LocDesc = rdr["LocDesc"].ToString(),
                                AccountCode = rdr["AccountID"].ToString(),
                                ChNumber = rdr["ChNumber"].ToString(),
                                AccountTitle = rdr["AccountName"].ToString(),
                                DocNo = Convert.ToInt32(rdr["FmtDocNo"]),
                                SubledgerCode = Convert.ToInt32(rdr["CustId"]),
                                SubledgerDesc = rdr["CustName"].ToString(),
                                Narration = rdr["Narration"].ToString(),
                                DocDate = Convert.ToDateTime(rdr["DocDate"]),
                                BookId = rdr["BookID"].ToString(),
                                Debit = Convert.ToDouble(rdr["Debit"]) / Convert.ToDouble(curRate),
                                Credit = Convert.ToDouble(rdr["Credit"]) / Convert.ToDouble(curRate),
                                //Amount = Convert.ToDouble(rdr["Amount"]),
                                Opening = Convert.ToDouble(rdr["Opening"]) / Convert.ToDouble(curRate),
                                LedgerDesc = rdr["LedgerDesc"].ToString()
                            };

                            generalLedgerDtoList.Add(generalLedgerDto);
                        }
                    }
                }
                // cn.Close();
            }
            return generalLedgerDtoList;
        }
    }
}