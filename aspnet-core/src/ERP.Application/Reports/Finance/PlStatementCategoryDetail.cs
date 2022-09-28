using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.SetupForms.GLPLCategory;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using ERP.Reports.Finance.Dto;

namespace ERP.Reports.Finance
{
    public class PlStatementCategoryDetail : ERPAppServiceBase
    {
        public PlStatementCategoryDetail()
        {

        }
        public List<GetReport> GetPlStatementCategoryDetail(string fromDate, string toDate, string headingText)
        {
            var prevFromDate = Convert.ToDateTime(fromDate).AddYears(-1);
            var prevToDate = Convert.ToDateTime(toDate).AddYears(-1);
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            var result = new List<GetReport>();
            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("select * from ( select year(GLTRH.DocDate) as Date, GLAMF.AccountID, GLAMF.AccountName, GLTRD.Amount from gltrh inner join" +
                                                " gltrd on gltrh.DetID = GLTRD.DetID and" +
                                                " GLTRH.TenantId = GLTRD.TenantId" +
                                                " inner join GLAMF on GLAMF.AccountID = gltrd.AccountID" +
                                                " and GLAMF.TenantId = GLTRD.TenantId" +
                                                " inner join GLPLCtg on GLAMF.AccountHeader = GLPLCtg.id" +
                                                " and GLAMF.TenantId = GLPLCtg.TenantId" +
                                                " where gltrd.TenantId = " + AbpSession.TenantId + " and gltrh.DocDate between '" + fromDate + "' and '" + toDate + "' and HeadingText = '" + headingText + "'" +
                                                " ) as a" +
                                                " full outer join ( select year(GLTRH.DocDate) as PrevDate, GLAMF.AccountID PrevAccountId, GLAMF.AccountName PrevAccountName, GLTRD.Amount as PrevAmount from gltrh inner join" +
                                                " gltrd on gltrh.DetID = GLTRD.DetID and" +
                                                " GLTRH.TenantId = GLTRD.TenantId" +
                                                " inner join GLAMF on GLAMF.AccountID = gltrd.AccountID" +
                                                " and GLAMF.TenantId = GLTRD.TenantId" +
                                                " inner join GLPLCtg on GLAMF.AccountHeader = GLPLCtg.id" +
                                                " and GLAMF.TenantId = GLPLCtg.TenantId where gltrd.TenantId = " + AbpSession.TenantId + " and gltrh.DocDate between '" + prevFromDate + "' and '" + prevToDate + "' and HeadingText = '" + headingText + "'" +
                                                ") as b on a.AccountID = b.PrevAccountId", cn))
                {
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@fromDate", fromDate.Date);
                    //cmd.Parameters.AddWithValue("@toDate", toDate.Date);
                    //cmd.Parameters.AddWithValue("@prevFromDate", fromDate.Date.AddYears(-1));
                    //cmd.Parameters.AddWithValue("@prevToDate", toDate.Date.AddYears(-1));
                    //cmd.Parameters.AddWithValue("@Loc", loc);
                    //cmd.Parameters.AddWithValue("@TenantID", AbpSession.TenantId);
                    cn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                      

                        while (dataReader.Read())
                        {
                            //decimal TotalAmount;
                            //decimal.TryParse(dataReader["BAL"].ToString(), out TotalAmount);

                            //decimal dueAmount;
                            //decimal.TryParse(dataReader["A1"].ToString(), out dueAmount);

                            //decimal Amount0_30;
                            //decimal.TryParse(dataReader["A2"].ToString(), out Amount0_30);

                            //decimal Amount31_60;
                            //decimal.TryParse(dataReader["A3"].ToString(), out Amount31_60);

                            //decimal Amount61_90;
                            //decimal.TryParse(dataReader["A4"].ToString(), out Amount61_90);

                            //decimal AboveAmount;
                            //decimal.TryParse(dataReader["A5"].ToString(), out AboveAmount);


                            result.Add(new GetReport
                            {
                                //Year = dataReader["year"] is DBNull ? 0 : Convert.ToInt32(dataReader["year"]),
                                AccountId = dataReader["AccountId"] is DBNull ? "" : dataReader["AccountId"].ToString(),
                                AccountName = dataReader["AccountName"] is DBNull ? "" : dataReader["AccountName"].ToString(),
                                Amount = dataReader["Amount"] is DBNull ? 0 : Convert.ToDouble(dataReader["Amount"]),
                                PrevAccountId = dataReader["PrevAccountId"] is DBNull ? "" : dataReader["PrevAccountId"].ToString(),
                                PrevAccountName = dataReader["PrevAccountName"] is DBNull ? "" : dataReader["PrevAccountName"].ToString(),
                                PrevAmount = dataReader["PrevAmount"] is DBNull ? 0 : Convert.ToDouble(dataReader["PrevAmount"]),
                            });
                        }
                    }
                }
                // cn.Close();

                // return result;
                //var profitandLossStatment = from h in _gltrHeaderRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId)
                //                            join d in _gltrDetailRepository.GetAll() on new { h.Id, h.TenantId } equals new { Id = d.DetID, TenantId = (int)d.TenantId }
                //                            join c in _chartofControlRepository.GetAll() on new { d.AccountID, d.TenantId } equals new { AccountID = c.Id, c.TenantId }
                //                            join pl in _plCategoryRepository.GetAll() on new { c.AccountHeader, c.TenantId } equals new { AccountHeader = (int?)pl.Id, TenantId = (int)pl.TenantId }
                //                            into plcat
                //                            from pl in plcat.DefaultIfEmpty()
                //                            where (h.DocDate >= FromDate && h.DocDate <= ToDate)
                //                            group new { d, c, pl } by new { c.AccountType, c.AccountHeader, pl.HeadingText } into g
                //                            select new ProfitAndLossStatmentDto { AccountType = g.Key.AccountType, AccountHeader = g.Key.AccountHeader, HeaderDescription = g.Key.HeadingText, Value = g.Sum(c => c.d.Amount) };



            }
            return result;

        }
    }
    public class GetReport
    {
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public double Amount { get; set; }
        public string PrevAccountId { get; set; }
        public string PrevAccountName { get; set; }
        public double PrevAmount { get; set; }
    }
}
