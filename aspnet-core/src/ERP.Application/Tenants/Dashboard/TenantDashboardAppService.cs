using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using ERP.Authorization;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using ERP.Reports;
using ERP.Tenants.Dashboard.Dto;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ERP.Tenants.Dashboard
{
    [DisableAuditing]
    [AbpAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class TenantDashboardAppService : ERPAppServiceBase, ITenantDashboardAppService
    {
        private readonly IRepository<GLCONFIG> _glConfigRepository;
        private readonly IRepository<GLBOOKS> _glBooksRepository;
        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;
        private readonly IRepository<GLTRDetail> _gltrDetailRepository;
        private readonly IRepository<ChartofControl, string> _ChartofAccountRepository;

        public TenantDashboardAppService(IRepository<GLTRHeader> gLTRHeaderrepository,
            IRepository<GLTRDetail> gLTRdetailRepository,
            IRepository<GLCONFIG> glconfigRepository,
            IRepository<GLBOOKS> glBooksRepository,
            IRepository<ChartofControl, string> chartofAccountRepository)
        {
            _gltrHeaderRepository = gLTRHeaderrepository;
            _gltrDetailRepository = gLTRdetailRepository;
            _glConfigRepository = glconfigRepository;
            _glBooksRepository = glBooksRepository;
            _ChartofAccountRepository = chartofAccountRepository;
        }
        public GetMemberActivityOutput GetMemberActivity()
        {
            return new GetMemberActivityOutput
            (
                DashboardRandomDataGenerator.GenerateMemberActivities()
            );
        }

        public GetDashboardDataOutput GetDashboardData(GetDashboardDataInput input)
        {
            var output = new GetDashboardDataOutput
            {
                //TotalProfit = DashboardRandomDataGenerator.GetRandomInt(5000, 9000),
                TotalProfit = (double)OpeingBalances("opening"),
                NewFeedbacks = (double)OpeingBalances("cashin"), //DashboardRandomDataGenerator.GetRandomInt(1000, 5000),
                NewOrders = (double)Math.Abs(OpeingBalances("cashout")),//DashboardRandomDataGenerator.GetRandomInt(100, 900),
                NewUsers = (double)(OpeingBalances("opening") + OpeingBalances("cashin") + OpeingBalances("cashout")), //DashboardRandomDataGenerator.GetRandomInt(50, 500),
                SalesSummary = DashboardRandomDataGenerator.GenerateSalesSummaryData(input.SalesSummaryDatePeriod),
                Expenses = DashboardRandomDataGenerator.GetRandomInt(5000, 10000),
                Growth = DashboardRandomDataGenerator.GetRandomInt(5000, 10000),
                Revenue = DashboardRandomDataGenerator.GetRandomInt(1000, 9000),
                TotalSales = DashboardRandomDataGenerator.GetRandomInt(10000, 90000),
                TransactionPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                NewVisitPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                BouncePercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                DailySales = DashboardRandomDataGenerator.GetRandomArray(30, 10, 50),
                ProfitShares = DashboardRandomDataGenerator.GetRandomPercentageArray(3)
            };

            return output;
        }

        public GetSalesSummaryOutput GetSalesSummary(GetSalesSummaryInput input)
        {
            return new GetSalesSummaryOutput(DashboardRandomDataGenerator.GenerateSalesSummaryData(input.SalesSummaryDatePeriod));
        }

        public GetRegionalStatsOutput GetRegionalStats()
        {
            return new GetRegionalStatsOutput(DashboardRandomDataGenerator.GenerateRegionalStat());
        }

        public GetGeneralStatsOutput GetGeneralStats()
        {
            return new GetGeneralStatsOutput
            {
                TransactionPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                NewVisitPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                BouncePercent = DashboardRandomDataGenerator.GetRandomInt(10, 100)
            };
        }

        public decimal OpeingBalances(string input)
        {
            decimal Data = 0;
            var accountdis = (from glc in _glConfigRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                              where new[] { "CP", "CR", "BR", "BP" }.Contains(glc.BookID)
                              select glc.AccountID
                        ).Distinct();

            var query = from h in _gltrHeaderRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                        join d in _gltrDetailRepository.GetAll() on new { DetID = h.Id, h.TenantId } equals new { d.DetID, d.TenantId }
                        where accountdis.Contains(d.AccountID)
                        select new
                        {
                            Debit = d.Amount > 0 ? d.Amount : 0,
                            Credit = d.Amount < 0 ? d.Amount : 0,
                        };



            if (input == "opening")
            {
                query = from h in _gltrHeaderRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                        join d in _gltrDetailRepository.GetAll() on new { DetID = h.Id, h.TenantId } equals new { d.DetID, d.TenantId }
                        where accountdis.Contains(d.AccountID) && DateTime.Parse(h.DocDate.ToLongDateString()) < DateTime.Parse(DateTime.Now.ToLongDateString())
                        select new
                        {

                            Debit = d.Amount > 0 ? d.Amount : 0,
                            Credit = d.Amount < 0 ? d.Amount : 0,
                        };

                Data = (decimal)(from o in query
                                 select o.Debit - o.Credit
                       ).Sum();



            }
            else if (input == "cashin")
            {
                Data = (decimal)(from o in query
                                 select o.Debit
                       ).Sum();
            }
            else if (input == "cashout")
            {
                Data = (decimal)(from o in query
                                 select o.Credit
                       ).Sum();
            }
            //else if (input == "Balance")
            //{
            //    decimal open = (decimal)(from h in _gltrHeaderRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
            //            join d in _gltrDetailRepository.GetAll() on new { DetID = h.Id, h.TenantId } equals new { d.DetID, d.TenantId }
            //            where accountdis.Contains(d.AccountID) && DateTime.Parse(h.DocDate.ToLongDateString()) < DateTime.Parse(DateTime.Now.ToLongDateString())
            //            select new
            //            {

            //                   d.Amount
            //            }).Sum();
            //    decimal Debit = Data = (decimal)(from o in query
            //                                     select  o.Debit 
            //           ).Sum();

            //    decimal Credit = (decimal)(from o in query
            //                               select o.Credit
            //           ).Sum();
            //    Data = open + Debit - Credit;
            //}
            return Data;
        }

        public async Task<ListResultDto<CashFlowSummary>> CashFlowSummary(GetCashFlowSummaryInput input)
        {

            var accountdis = (from glc in _glConfigRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                              where new[] { "CP", "CR" }.Contains(glc.BookID)
                              select glc.AccountID
                        ).Distinct();

            var query = from h in _gltrHeaderRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)

                        join d in _gltrDetailRepository.GetAll() on new { DetID = h.Id, h.TenantId } equals new { d.DetID, d.TenantId }
                        where accountdis.Contains(d.AccountID)

                        select new
                        {
                            d.AccountID,
                            h.DocDate,
                            Debit = d.Amount > 0 ? d.Amount : 0,
                            Credit = d.Amount < 0 ? d.Amount : 0,
                            d.TenantId
                        };

            var data = from o in query
                       group o by new { day = o.DocDate.Day, month = o.DocDate.Month, year = o.DocDate.Year } into g
                       select new CashFlowSummary
                       {
                           DocDate = string.Format("{0}-{1}-{2}", g.Key.day, g.Key.month, g.Key.year),
                           Debit = (decimal)g.Sum(o => o.Debit),
                           Credit = (decimal)g.Sum(o => o.Credit)
                       };

            switch (input.CashFlowSummaryDatePeriod)
            {
                case CashFlowSummaryDatePeriod.Daily:
                    data = from o in query
                           group o by new { day = o.DocDate.Day, month = o.DocDate.Month, year = o.DocDate.Year } into g
                           select new CashFlowSummary
                           {
                               DocDate = string.Format("{0}-{1}-{2}", g.Key.day, g.Key.month, g.Key.year),
                               Debit = (decimal)g.Sum(o => o.Debit),
                               Credit = (decimal)g.Sum(o => o.Credit)
                           };
                    break;
                case CashFlowSummaryDatePeriod.Weekly:
                    data = from o in query
                           group o by new { Weekly = 1 + (o.DocDate.DayOfYear - 1) / 7, month = o.DocDate.Month, year = o.DocDate.Year } into g
                           select new CashFlowSummary
                           {
                               DocDate = string.Format("{0}-{1}", g.Key.Weekly, g.Key.year),
                               Debit = (decimal)g.Sum(o => o.Debit),
                               Credit = (decimal)g.Sum(o => o.Credit)
                           };
                    break;
                case CashFlowSummaryDatePeriod.Monthly:
                    data = from o in query
                           group o by new { month = o.DocDate.Month, year = o.DocDate.Year } into g
                           select new CashFlowSummary
                           {
                               DocDate = string.Format("{0}-{1}", g.Key.month, g.Key.year),
                               Debit = (decimal)g.Sum(o => o.Debit),
                               Credit = (decimal)g.Sum(o => o.Credit)
                           };
                    break;
                default:
                    data = from o in query
                           group o by new { day = o.DocDate.Day, month = o.DocDate.Month, year = o.DocDate.Year } into g
                           select new CashFlowSummary
                           {
                               DocDate = string.Format("{0}-{1}-{2}", g.Key.day, g.Key.month, g.Key.year),
                               Debit = (decimal)g.Sum(o => o.Debit),
                               Credit = (decimal)g.Sum(o => o.Credit)
                           };
                    break;
            }




            return new ListResultDto<CashFlowSummary>(
               await data.ToListAsync()

                );
        }

        public async Task<ListResultDto<BankStatusSummary>> BankStatusSummary(GetBankStatusSummaryInput input)
        {

            var accountdis = (from glc in _glConfigRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                              where new[] { "BR", "BP" }.Contains(glc.BookID)
                              select glc.AccountID
                        ).Distinct();

            var query = from h in _gltrHeaderRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)

                        join d in _gltrDetailRepository.GetAll() on new { DetID = h.Id, h.TenantId } equals new { d.DetID, d.TenantId }
                        where accountdis.Contains(d.AccountID)
                        //where new[] { "CP", "CR" }.Contains(c.BookID)
                        select new
                        {
                            d.AccountID,
                            h.DocDate,
                            Debit = d.Amount > 0 ? d.Amount : 0,
                            Credit = d.Amount < 0 ? d.Amount : 0,
                            d.TenantId
                        };

            var data = from o in query
                       group o by new { day = o.DocDate.Day, month = o.DocDate.Month, year = o.DocDate.Year } into g
                       select new BankStatusSummary
                       {
                           DocDate = string.Format("{0}-{1}-{2}", g.Key.day, g.Key.month, g.Key.year),
                           Debit = (decimal)g.Sum(o => o.Debit),
                           Credit = (decimal)g.Sum(o => o.Credit)
                       };

            switch (input.BankStatusSummaryDatePeriod)
            {
                case BankStatusSummaryDatePeriod.Daily:
                    data = from o in query
                           group o by new { day = o.DocDate.Day, month = o.DocDate.Month, year = o.DocDate.Year } into g
                           select new BankStatusSummary
                           {
                               DocDate = string.Format("{0}-{1}-{2}", g.Key.day, g.Key.month, g.Key.year),
                               Debit = (decimal)g.Sum(o => o.Debit),
                               Credit = (decimal)g.Sum(o => o.Credit)
                           };
                    break;
                case BankStatusSummaryDatePeriod.Weekly:
                    data = from o in query
                           group o by new { Weekly = 1 + (o.DocDate.DayOfYear - 1) / 7, month = o.DocDate.Month, year = o.DocDate.Year } into g
                           select new BankStatusSummary
                           {
                               DocDate = string.Format("{0}-{1}", g.Key.Weekly, g.Key.year),
                               Debit = (decimal)g.Sum(o => o.Debit),
                               Credit = (decimal)g.Sum(o => o.Credit)
                           };
                    break;
                case BankStatusSummaryDatePeriod.Monthly:
                    data = from o in query
                           group o by new { month = o.DocDate.Month, year = o.DocDate.Year } into g
                           select new BankStatusSummary
                           {
                               DocDate = string.Format("{0}-{1}", g.Key.month, g.Key.year),
                               Debit = (decimal)g.Sum(o => o.Debit),
                               Credit = (decimal)g.Sum(o => o.Credit)
                           };
                    break;
                default:
                    data = from o in query
                           group o by new { day = o.DocDate.Day, month = o.DocDate.Month, year = o.DocDate.Year } into g
                           select new BankStatusSummary
                           {
                               DocDate = string.Format("{0}-{1}-{2}", g.Key.day, g.Key.month, g.Key.year),
                               Debit = (decimal)g.Sum(o => o.Debit),
                               Credit = (decimal)g.Sum(o => o.Credit)
                           };
                    break;
            }




            return new ListResultDto<BankStatusSummary>(
               await data.ToListAsync()

                );
        }

        public GetDashboardDataOutput getDashboardheaderStats()
        {
            //var cash = (from glamf in _ChartofAccountRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
            //            from gltrd in _gltrDetailRepository.GetAll().Where(x => x.TenantId == glamf.TenantId && x.AccountID == glamf.Id && glamf.AccNature == "CASH")
            //            from gltrh in _gltrHeaderRepository.GetAll().Where(x => x.Id == gltrd.DetID && x.DocDate == DateTime.Now.Date)
            //            select new GetDashboardDataOutput
            //            {
            //                CashBalance = gltrd.Amount,
            //                BankBalance = 0.0

            //            }.CashBalance).Sum();

            //var bank = (from glamf in _ChartofAccountRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
            //            from gltrd in _gltrDetailRepository.GetAll().Where(x => x.TenantId == glamf.TenantId && x.AccountID == glamf.Id && glamf.AccNature == "BANK")
            //            from gltrh in _gltrHeaderRepository.GetAll().Where(x => x.Id == gltrd.DetID && x.DocDate == DateTime.Now.Date)
            //            select new GetDashboardDataOutput
            //            {
            //                CashBalance = 0.0,
            //                BankBalance = gltrd.Amount

            //            }.BankBalance).Sum();


            var tenantId = AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<PostDatedChequesData> postDatedChequesDataDtoList = new List<PostDatedChequesData>();
            var objDashboardStats = new GetDashboardDataOutput();
            using (SqlConnection cn = new SqlConnection(str))
            {

                using (SqlCommand cmd = new SqlCommand("dbo.DB_CashPosition", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DocDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@TenantID", tenantId);
                    cn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {


                        while (rdr.Read())
                        {
                            objDashboardStats.CashBalance = Convert.ToInt32(rdr["CashBal"]);
                            objDashboardStats.BankBalance = Convert.ToInt32(rdr["BankBal"]);
                            objDashboardStats.TotalBalance = 0;// Convert.ToInt32(rdr["TotalBal"]);
                        };
                    }
                }
            }
            return objDashboardStats;
        }

        public List<PostDatedChequesData> GetPostDatedChequesRecieved()
        {
            var tenantId = AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<PostDatedChequesData> postDatedChequesDataDtoList = new List<PostDatedChequesData>();
            using (SqlConnection cn = new SqlConnection(str))
            {


                using (SqlCommand cmd = new SqlCommand("dbo.DB_PDC", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@TypeID", 1);
                    cmd.Parameters.AddWithValue("@ChStatus", 1);
                    cmd.Parameters.AddWithValue("@TenantID", tenantId);
                    cn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            PostDatedChequesData postDatedChequesData = new PostDatedChequesData()
                            {
                                DocId = Convert.ToInt32(rdr["DocID"]),
                                ChequeDate = Convert.ToDateTime(rdr["ChequeDate"]),
                                PartyID = Convert.ToInt32(rdr["PartyID"]),
                                SubAccName = rdr["SubAccName"].ToString(),
                                ChequeAmt = Convert.ToInt32(rdr["ChequeAmt"])
                            };
                            postDatedChequesDataDtoList.Add(postDatedChequesData);
                        }
                    }
                }
                // cn.Close();
            }

            return postDatedChequesDataDtoList;

        }

        public List<PostDatedChequesData> GetPostDatedChequesIssued()
        {
            var tenantId = AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<PostDatedChequesData> postDatedChequesDataDtoList = new List<PostDatedChequesData>();
            using (SqlConnection cn = new SqlConnection(str))
            {

                using (SqlCommand cmd = new SqlCommand("dbo.DB_PDC", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@TypeID", 0);
                    cmd.Parameters.AddWithValue("@ChStatus", 2);
                    cmd.Parameters.AddWithValue("@TenantID", tenantId);
                    cn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            PostDatedChequesData postDatedChequesData = new PostDatedChequesData()
                            {
                                DocId = Convert.ToInt32(rdr["DocID"]),
                                ChequeDate = Convert.ToDateTime(rdr["ChequeDate"]),
                                PartyID = Convert.ToInt32(rdr["PartyID"]),
                                SubAccName = rdr["SubAccName"].ToString(),
                                ChequeAmt = Convert.ToInt32(rdr["ChequeAmt"])
                            };
                            postDatedChequesDataDtoList.Add(postDatedChequesData);
                        }
                    }
                }
                // cn.Close();
            }
            return postDatedChequesDataDtoList;

        }


        public List<BankOverDraftData> GetBankOverDraft()
        {
            var tenantId = AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<BankOverDraftData> bankOverDraftsList = new List<BankOverDraftData>();
            using (SqlConnection cn = new SqlConnection(str))
            {


                using (SqlCommand cmd = new SqlCommand("dbo.DB_BankOD", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TenantID", tenantId);
                    cn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            BankOverDraftData bankOverDraft = new BankOverDraftData()
                            {
                                BankID = rdr["BankID"].ToString(),
                                BANKNAME = rdr["BANKNAME"].ToString(),
                                IDACCTBANK = rdr["IDACCTBANK"].ToString(),
                                BalanceLimit = Convert.ToInt32(rdr["BalanceLimit"]),
                                ODLIMIT = Convert.ToInt32(rdr["ODLIMIT"]),
                                UsedLimit = Convert.ToInt32(rdr["UsedLimit"])
                            };
                            bankOverDraftsList.Add(bankOverDraft);
                        }
                    }
                }
                // cn.Close();
            }
            return bankOverDraftsList;

        }
        public List<PartyBalanceData> GetPartyRecieveable()
        {
            var tenantId = AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<PartyBalanceData> partyBalanceDataList = new List<PartyBalanceData>();
            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.DB_PartyBal", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TypeID", 1);
                    cmd.Parameters.AddWithValue("@TenantID", tenantId);
                    cn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            PartyBalanceData partyBalanceData = new PartyBalanceData()
                            {
                                SubAccID = rdr["SubAccID"].ToString(),
                                SubAccName = rdr["SubAccName"].ToString(),
                                Amount = Convert.ToInt32(rdr["Amount"])

                            };
                            partyBalanceDataList.Add(partyBalanceData);
                        }
                    }
                }
                // cn.Close();
            }
            return partyBalanceDataList;

        }



        public List<PartyBalanceData> GetPartyPayable()
        {
            var tenantId = AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<PartyBalanceData> partyBalanceDataList = new List<PartyBalanceData>();
            using (SqlConnection cn = new SqlConnection(str))
            {


                using (SqlCommand cmd = new SqlCommand("dbo.DB_PartyBal", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TypeID", 2);
                    cmd.Parameters.AddWithValue("@TenantID", tenantId);
                    cn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            PartyBalanceData partyBalanceData = new PartyBalanceData()
                            {
                                SubAccID = rdr["SubAccID"].ToString(),
                                SubAccName = rdr["SubAccName"].ToString(),
                                Amount = Convert.ToInt32(rdr["Amount"])

                            };
                            partyBalanceDataList.Add(partyBalanceData);
                        }
                    }
                }
                // cn.Close();

            }
            return partyBalanceDataList;

        }


        public List<StockBalancesData> GetStockBalance()
        {
            var tenantId = AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<StockBalancesData> stockBalanceDataList = new List<StockBalancesData>();
            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.DB_StockPosition", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DocDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@TenantID", tenantId);
                    cn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            StockBalancesData stockBalanceData = new StockBalancesData()
                            {
                                accountid = rdr["accountid"].ToString(),
                                AccountName = rdr["AccountName"].ToString(),
                                Balance = Convert.ToInt32(rdr["Balance"])

                            };
                            stockBalanceDataList.Add(stockBalanceData);
                        }
                    }
                }
                // cn.Close();
            }
            return stockBalanceDataList;

        }
        public List<CashDetailData> GetCashBalance()
        {
            var tenantId = AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<CashDetailData> CashDetailDataList = new List<CashDetailData>();
            using (SqlConnection cn = new SqlConnection(str))
            {

                using (SqlCommand cmd = new SqlCommand("dbo.DB_CASHDETL", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", "cash");
                    cmd.Parameters.AddWithValue("@TenantID", tenantId);
                    cn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            CashDetailData CashdetailData = new CashDetailData()
                            {
                                accountid = rdr["accountid"].ToString(),
                                AccountName = rdr["AccountName"].ToString(),
                                Balance = Convert.ToInt32(rdr["Balance"])

                            };
                            CashDetailDataList.Add(CashdetailData);
                        }
                    }
                }
                // cn.Close();
            }
            return CashDetailDataList;

        }
        public List<CashDetailData> GetBankBalance()
        {
            var tenantId = AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<CashDetailData> CashDetailDataList = new List<CashDetailData>();
            using (SqlConnection cn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand("dbo.DB_CASHDETL", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", "bank");
                cmd.Parameters.AddWithValue("@TenantID", tenantId);
                cn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        CashDetailData CashdetailData = new CashDetailData()
                        {
                            accountid = rdr["accountid"].ToString(),
                            AccountName = rdr["AccountName"].ToString(),
                            Balance = Convert.ToInt32(rdr["Balance"])

                        };
                        CashDetailDataList.Add(CashdetailData);
                    }
                }
                // cn.Close();
            }
            return CashDetailDataList;

        }

    }
}