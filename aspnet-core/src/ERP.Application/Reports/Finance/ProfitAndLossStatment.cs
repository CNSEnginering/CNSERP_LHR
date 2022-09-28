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
    public class ProfitAndLossStatment : ERPAppServiceBase, IProfitAndLossStatment
    {

        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;
        private readonly IRepository<GLTRDetail> _gltrDetailRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<PLCategory> _plCategoryRepository;
        public ProfitAndLossStatment(IRepository<GLTRHeader> gltrHeaderRepository, IRepository<GLTRDetail> gltrDetailRepository, IRepository<ChartofControl, string> chartofControlRepository, IRepository<PLCategory> plCategoryRepository)
        {
            _gltrHeaderRepository = gltrHeaderRepository;
            _gltrDetailRepository = gltrDetailRepository;
            _chartofControlRepository = chartofControlRepository;
            _plCategoryRepository = plCategoryRepository;
        }
        public List<ProfitAndLossStatmentDto> GetCashFlowStatment(DateTime fromDate, DateTime toDate, int loc)
        {

            string str = ConfigurationManager.AppSettings["ConnectionString"];
            var result = new List<ProfitAndLossStatmentDto>();
            using (SqlConnection cn = new SqlConnection(str))
            {

                using (SqlCommand cmd = new SqlCommand("SP_CFYear", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromDate", fromDate.Date);
                    cmd.Parameters.AddWithValue("@toDate", toDate.Date);
                    cmd.Parameters.AddWithValue("@prevFromDate", fromDate.Date.AddYears(-1));
                    cmd.Parameters.AddWithValue("@prevToDate", toDate.Date.AddYears(-1));
                    cmd.Parameters.AddWithValue("@Loc", loc);
                    cmd.Parameters.AddWithValue("@TenantID", AbpSession.TenantId);
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


                            result.Add(new ProfitAndLossStatmentDto
                            {
                                //Year = dataReader["year"] is DBNull ? 0 : Convert.ToInt32(dataReader["year"]),
                                HeadingText = dataReader["CFTypeDesc"] is DBNull ? "" : dataReader["CFTypeDesc"].ToString(),
                                Amount = dataReader["Amount"] is DBNull ? 0 : Convert.ToDouble(dataReader["Amount"]),
                                PrevAmount = dataReader["PrevAmount"] is DBNull ? 0 : Convert.ToDouble(dataReader["PrevAmount"]),
                                TypeId = dataReader["CFType"] is DBNull ? "" : dataReader["CFType"].ToString(),
                                GLPLCtGId = dataReader["CFGID"] is DBNull ? 0 : Convert.ToInt32(dataReader["CFGID"])
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
        public List<ProfitAndLossStatmentDto> GetProfitAndLossStatment(DateTime fromDate, DateTime toDate, int loc)
        {

            string str = ConfigurationManager.AppSettings["ConnectionString"];
            var result = new List<ProfitAndLossStatmentDto>();
            using (SqlConnection cn = new SqlConnection(str))
            {

                using (SqlCommand cmd = new SqlCommand("SP_PLYear", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromDate", fromDate.Date);
                    cmd.Parameters.AddWithValue("@toDate", toDate.Date);
                    cmd.Parameters.AddWithValue("@prevFromDate", fromDate.Date.AddYears(-1));
                    cmd.Parameters.AddWithValue("@prevToDate", toDate.Date.AddYears(-1));
                    cmd.Parameters.AddWithValue("@Loc", loc);
                    cmd.Parameters.AddWithValue("@TenantID", AbpSession.TenantId);
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


                            result.Add(new ProfitAndLossStatmentDto
                            {
                                //Year = dataReader["year"] is DBNull ? 0 : Convert.ToInt32(dataReader["year"]),
                                HeadingText = dataReader["HeadingText"] is DBNull ? "" : dataReader["HeadingText"].ToString(),
                                Amount = dataReader["Amount"] is DBNull ? 0 : Convert.ToDouble(dataReader["Amount"]),
                                PrevAmount = dataReader["PrevAmount"] is DBNull ? 0 : Convert.ToInt32(dataReader["PrevAmount"]),
                                TypeId = dataReader["TypeID"] is DBNull ? "" : dataReader["TypeID"].ToString(),
                                GLPLCtGId = dataReader["GLPLCtGId"] is DBNull ? 0 : Convert.ToInt32(dataReader["GLPLCtGId"])
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
}
