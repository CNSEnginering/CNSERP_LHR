using Abp.Domain.Repositories;
using ERP.GeneralLedger.Transaction;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ERP.Reports.Finance
{
    public class PDCSubReportAppService : ERPReportAppServiceBase
    {
        public PDCSubReportAppService(IRepository<GlCheque> glChequeRepository)
        {
        }
        public List<PDCSubReport> GetPDCSubReport(int subLedgerCode, int tenantId, string accId)
        {
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<PDCSubReport> pdcSubReportList = new List<PDCSubReport>();

            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("select * from glpdc where TenantID = " + tenantId + " and PartyID = " + subLedgerCode + " and AccountID = '" + accId + "' and ChequeStatus in (1,6,2)", cn))
                {
                    cn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var chequeStatus = (int)rdr["ChequeStatus"];
                            string status = "";
                            if (chequeStatus == 1)
                            {
                                status = "Collected";
                            }
                            if (chequeStatus == 2)
                            {
                                status = "Issued";
                            }
                            else if (chequeStatus == 3)
                            {
                                status = "Deposited";
                            }
                            else if (chequeStatus == 4)
                            {
                                status = "Cleared";
                            }
                            else if (chequeStatus == 5)
                            {
                                status = "Cancelled";
                            }
                            else if (chequeStatus == 6)
                            {
                                status = "Holded";
                            }
                            else if (chequeStatus == 7)
                            {
                                status = "Bounced";
                            }
                            pdcSubReportList.Add(new PDCSubReport()
                            {
                                DocNo = (int)(rdr["DocId"]),
                                ChequeNo = rdr["ChequeNo"].ToString(),
                                ChequeAmount = rdr["ChequeAmt"] is DBNull ? 0 : (double)(rdr["ChequeAmt"]),
                                Balance = rdr["ChequeAmt"] is DBNull ? 0 : (double)(rdr["ChequeAmt"]),
                                Narration = "PDC-" + (rdr["TypeID"].ToString() == "1" ? "AR" : "AP") + "-" + rdr["DocId"],
                                Status = status,
                                Date = Convert.ToDateTime(rdr["EntryDate"]),
                                Type = rdr["TypeID"].ToString() == "1" ? "AR" : "AP",
                                ChequeDate = Convert.ToDateTime(rdr["ChequeDate"])
                            });
                        }
                    }
                }
            }
            return pdcSubReportList;
        }
    }

    public class PDCSubReport
    {
        public string Type { get; set; }
        public DateTime ChequeDate { get; set; }
        public int DocNo { get; set; }
        public DateTime Date { get; set; }
        public string ChequeNo { get; set; }
        public double ChequeAmount { get; set; }
        public double Balance { get; set; }
        public string Narration { get; set; }
        public string Status { get; set; }
    }
}
