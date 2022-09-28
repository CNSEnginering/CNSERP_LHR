using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace ERP.Reports.Finance
{
    public class PlStatementVoucherDetail : ERPAppServiceBase
    {
        public PlStatementVoucherDetail()
        {

        }
        public List<PlStatementVoucherDetailReport> GetPlStatementVoucherDetailReport(string acccId, string fromDate, string toDate)
        {
            var prevFromDate = Convert.ToDateTime(fromDate).AddYears(-1).Year.ToString() + "/" +
                Convert.ToDateTime(fromDate).AddYears(-1).Month.ToString() + "/" +
                Convert.ToDateTime(fromDate).AddYears(-1).Day.ToString();

            var prevToDate = Convert.ToDateTime(toDate).AddYears(-1).Year.ToString() + "/" +
                Convert.ToDateTime(toDate).AddYears(-1).Month.ToString() + "/" +
                Convert.ToDateTime(toDate).AddYears(-1).Day.ToString();

            string str = ConfigurationManager.AppSettings["ConnectionString"];
            var result = new List<PlStatementVoucherDetailReport>();
            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand(" select * from  ( select GLTRH.DetID, GLTRH.BookID, GLTRH.DocDate,GLTRH.Narration, GLTRH.FmtDocNo, GLTRD.Amount" +
                                                " from gltrd inner join GLTRH on GLTRH.DetID = GLTRD.DetID and GLTRH.TenantId = GLTRD.TenantId " +
                                                " where AccountID = '" + acccId + "' and GLTRH.TenantId = " + AbpSession.TenantId + " and DocDate between '" + fromDate + "' and '" + toDate + "'" +
                                                " ) as a full outer join ( " +
                                                " select GLTRH.DetID as PrevDetId, GLTRH.BookID as PrevBookId, GLTRH.DocDate as PrevDocDate, GLTRH.FmtDocNo as PrevFmtDocNo, GLTRD.Amount as PrevAmount " +
                                                " from gltrd inner join GLTRH on GLTRH.DetID = GLTRD.DetID and GLTRH.TenantId = " +
                                                " GLTRD.TenantId where AccountID = '" + acccId + "' and GLTRH.TenantId = " + AbpSession.TenantId + " and DocDate between '" + prevFromDate + "' and '" + prevToDate + "'" +
                                                " ) as b on a.BookID = b.PrevBookId", cn))
                {
                    cn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                       

                        while (dataReader.Read())
                        {
                            result.Add(new PlStatementVoucherDetailReport
                            {
                                BookId = dataReader["BookId"] is DBNull ? "" : dataReader["BookId"].ToString(),
                                FmtDocNo = dataReader["FmtDocNo"] is DBNull ? 0 : Convert.ToInt32(dataReader["FmtDocNo"]),
                                Amount = dataReader["Amount"] is DBNull ? 0 : Convert.ToDouble(dataReader["Amount"]),
                                PrevAmount = dataReader["PrevAmount"] is DBNull ? 0 : Convert.ToDouble(dataReader["PrevAmount"]),
                                DocDate = Convert.ToDateTime(dataReader["DocDate"]),
                                Narration = dataReader["Narration"] is DBNull ? "" : dataReader["Narration"].ToString()
                            });
                        }
                    }
                }
                // cn.Close();
            }
            return result;

        }
    }

    public class PlStatementVoucherDetailReport
    {
        public string BookId { get; set; }
        public DateTime DocDate { get; set; }
        public int FmtDocNo { get; set; }
        public double Amount { get; set; }
        public double PrevAmount { get; set; }
        public string Narration { get; set; }
    }
}
