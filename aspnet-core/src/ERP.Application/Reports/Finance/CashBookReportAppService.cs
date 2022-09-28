using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using ERP.GeneralLedger.SetupForms;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Linq.Extensions;
using Abp.Application.Services.Dto;
using Newtonsoft.Json;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using static ERP.Reports.Finance.GeneralLedgerReportAppService;

namespace ERP.Reports.Finance
{
    public class CashBookReportAppService : ERPReportAppServiceBase, ICashBookReportAppService
    {
        private readonly IRepository<GLCONFIG> _glConfigRepository;
        private readonly IRepository<GLBOOKS> _glBooksRepository;
        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;
        private readonly IRepository<GLTRDetail> _gltrDetailRepository;
        private readonly IRepository<ChartofControl, string> _ChartofAccountRepository;
        public CashBookReportAppService(IRepository<GLTRHeader> gLTRHeaderrepository,
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


        public List<GeneralLedgerDto> GetCashBook(DateTime fromDate, DateTime toDate, string fromAC, string toAC, string status, int locId, int? curRate, bool Cashbook)
        {
            var tenantId = AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<GeneralLedgerDto> generalLedgerDtoList = new List<GeneralLedgerDto>();
            using (SqlConnection cn = new SqlConnection(str))
            {
                string voucherStatus = "";
                
                string query = "";
                if (Cashbook)
                    query = "SP_CashBookRpt";
                else
                    query = "SP_BankBookRpt";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromDate", fromDate.Date);
                    cmd.Parameters.AddWithValue("@toDate", toDate.Date);
                    cmd.Parameters.AddWithValue("@tenantId", tenantId);
                    cmd.Parameters.AddWithValue("@locId", locId);
                    cmd.Parameters.AddWithValue("@fromAcc", fromAC);
                    cmd.Parameters.AddWithValue("@toAcc", toAC);
                    if (status == "Approved")
                    {
                        cmd.Parameters.AddWithValue("@Approved", 1);
                        cmd.Parameters.AddWithValue("@Posted", 0);
                    }
                    else if (status == "Posted")
                    {
                        cmd.Parameters.AddWithValue("@Approved", 0);
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
                            if (Convert.ToBoolean(rdr["Approved"]) == true && Convert.ToBoolean(rdr["Posted"]) == true)
                            {
                                voucherStatus = "Approved + Posted";
                            }
                            else if (Convert.ToBoolean(rdr["Approved"]) == true)
                            {
                                voucherStatus = "Approved";
                            }
                            else if (Convert.ToBoolean(rdr["Posted"]) == true)
                            {
                                voucherStatus = "Posted";
                            }
                            GeneralLedgerDto generalLedgerDto = new GeneralLedgerDto()
                            {
                                AccountCode = rdr["AccountCode"].ToString(),
                                AccountTitle = rdr["AccountTitle"].ToString(),
                                DocNo = Convert.ToInt32(rdr["DocNo"]),
                                SubledgerCode = Convert.ToInt32(rdr["SubledgerCode"]),
                                SubledgerDesc = rdr["SubledgerDesc"].ToString(),
                                Narration = rdr["Narration"].ToString(),
                                DocDate = Convert.ToDateTime(rdr["DocDate"]),
                                BookId = rdr["BookID"].ToString(),
                                ConfigId = Convert.ToInt32(rdr["ConfigID"]),
                                Debit = Convert.ToDouble(rdr["Debit"]) / Convert.ToDouble(curRate),
                                Credit = Convert.ToDouble(rdr["Credit"]) / Convert.ToDouble(curRate),
                                Amount = Convert.ToDouble(rdr["Amount"]) / Convert.ToDouble(curRate),
                                Opening = Convert.ToDouble(rdr["Opening"]) / Convert.ToDouble(curRate),
                                LocId = Convert.ToInt32(rdr["LocID"]),
                                LocDesc = rdr["LocDesc"].ToString(),
                                ChNo = rdr["ChNumber"].ToString(),
                                Status = voucherStatus
                            };

                            generalLedgerDtoList.Add(generalLedgerDto);
                        }
                    }
                }
                // // cn.Close();
            }
            //IQueryable<GLTRHeader> gltrHeader;
            //if (status == "Approved")
            //{
            //    //Approved Only
            //    gltrHeader = _gltrHeaderRepository.GetAll().Where(e => e.TenantId == tenantId && e.Approved == true && e.Posted == false).Where(d => d.DocDate.Date <= toDate.Date);
            //}
            //else if (status == "Posted")
            //{
            //    //Posted
            //    gltrHeader = _gltrHeaderRepository.GetAll().Where(e => e.TenantId == tenantId && e.Approved == false && e.Posted == true).Where(d => d.DocDate.Date <= toDate.Date);
            //}
            //else
            //{
            //    //Approved & Posted
            //    gltrHeader = _gltrHeaderRepository.GetAll().Where(e => e.TenantId == tenantId && e.Posted == true && e.Approved == true).Where(d => d.DocDate <= toDate.Date);
            //}
            //var gltrDetail = _gltrDetailRepository.GetAll().Where(e => e.TenantId == tenantId).Where(d => d.AccountID.CompareTo(fromAC) >= 0 && d.AccountID.CompareTo(toAC) <= 0);
            //var chartOfAc = _chartofControlRepository.GetAll().Where(e => e.TenantId == tenantId);
            //var subLedger = _accountSubLedgerRepository.GetAll().Where(e => e.TenantId == tenantId);
            //IQueryable<GLLocation> locations;
            //if (locId == 0)
            //{
            //    locations = _glLocationRepository.GetAll().Where(o => o.TenantId == tenantId);
            //}
            //else
            //{
            //    locations = _glLocationRepository.GetAll().Where(o => o.TenantId == tenantId && o.LocId == locId);
            //}

            //var generalLedger = (from ca in chartOfAc
            //                     join d in gltrDetail on ca.Id equals d.AccountID
            //                     join h in gltrHeader on d.DetID equals h.Id
            //                     join l in locations on h.LocId equals l.LocId
            //                     join sl in subLedger on new { X = d.AccountID, Y = (int)d.SubAccID } equals new { X = sl.AccountID, Y = sl.Id } into sb
            //                     from p in sb.DefaultIfEmpty()
            //                     orderby h.DocDate
            //                     select new GeneralLedgerDto
            //                     {
            //                         AccountCode = ca.Id,
            //                         AccountTitle = ca.AccountName != null ? ca.AccountName : "",
            //                         DocNo = h.DocNo,
            //                         SubledgerCode = d.SubAccID == null ? 0 : (int)d.SubAccID,
            //                         SubledgerDesc = p.SubAccName != null ? p.SubAccName : "",
            //                         Narration = d.Narration != null ? d.Narration : "",
            //                         DocDate = h.DocDate,
            //                         BookId = h.BookID,
            //                         ConfigId = h.ConfigID,
            //                         Debit = (double)(d.Amount > 0 ? d.Amount : 0),
            //                         Credit = (double)(d.Amount < 0 ? -(d.Amount) : 0),
            //                         Amount = (double)(d.Amount),
            //                         LocId = h.LocId,
            //                         LocDesc = l.LocDesc != null ? l.LocDesc : ""
            //                     }).ToList();

            //string pjson = JsonConvert.SerializeObject(generalLedger);
            return generalLedgerDtoList.ToList();
        }
        //public List<CashBookDto> GetAll(DateTime? FromDate, DateTime? ToDate, string FromAccount, string ToAccount, int TenantID, bool CashBook)
        //{

        //    List<string> cash = new List<string>() { "CP", "CR" };
        //    List<string> Bank = new List<string>() { "CP", "CR" };
        //    var query = //CashBook ? 

        //    (from h in _gltrHeaderRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.Approved == true)
        //            .WhereIf(FromDate != null, e => Convert.ToDateTime(e.DocDate.ToShortDateString()) < Convert.ToDateTime(Convert.ToDateTime(FromDate.ToString()).ToShortDateString()))
        //      join d in _gltrDetailRepository.GetAll()
        //          .WhereIf(!string.IsNullOrWhiteSpace(FromAccount), e => false || e.AccountID.CompareTo(FromAccount) >= 0)
        //          .WhereIf(!string.IsNullOrWhiteSpace(ToAccount), e => false || e.AccountID.CompareTo(ToAccount) <= 0)
        //          on new { DetID = h.Id, h.TenantId } equals new { d.DetID, d.TenantId }
        //         //join c in _glConfigRepository.GetAll()
        //         //      on new { d.AccountID, d.TenantId } equals new { c.AccountID, c.TenantId }
        //      where CashBook ? new[] { "CP", "CR" }.Contains(h.BookID) : new[] { "BP", "BR" }.Contains(h.BookID)
        //      select new
        //      {
        //          DetID = 0,
        //          DocDate = Convert.ToDateTime(Convert.ToDateTime(FromDate.ToString()).ToShortDateString()),
        //          BookID = "",
        //          AccountID =  d.AccountID == null ? "" : d.AccountID,
        //          Narration = "Opening Balance",
        //          Debit = (double?)0.00,
        //          Credit = (double?)0.00,
        //          Opening = d.Amount == null ? 0.00 : d.Amount,
        //          d.TenantId })

        //     .Concat(from h in _gltrHeaderRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.Approved == true)
        //            .WhereIf(FromDate != null, e => Convert.ToDateTime(e.DocDate.ToShortDateString()) >= Convert.ToDateTime(Convert.ToDateTime(FromDate.ToString()).ToShortDateString()))
        //             .WhereIf(ToDate != null, e => Convert.ToDateTime(e.DocDate.ToShortDateString()) <= Convert.ToDateTime(Convert.ToDateTime(ToDate.ToString()).ToShortDateString()))
        //              join d in _gltrDetailRepository.GetAll()
        //                  .WhereIf(!string.IsNullOrWhiteSpace(FromAccount), e => false || e.AccountID.CompareTo(FromAccount) >= 0)
        //                  .WhereIf(!string.IsNullOrWhiteSpace(ToAccount), e => false || e.AccountID.CompareTo(ToAccount) <= 0)
        //                  on new { DetID = h.Id, h.TenantId } equals new { d.DetID, d.TenantId }

        //              where CashBook ? new[] { "CP", "CR" }.Contains(h.BookID) : new[] { "BP", "BR" }.Contains(h.BookID)
        //              select new
        //              {
        //                  d.DetID,
        //                  h.DocDate,
        //                  h.BookID,
        //                  d.AccountID,
        //                  d.Narration,
        //                  Debit = d.Amount > 0 ? d.Amount : 0,
        //                  Credit = d.Amount < 0 ? d.Amount : 0,
        //                  Opening = (double?)0.00,
        //                  d.TenantId
        //              });



        //    var newquery = from o in query.ToList()
        //                   group o by new { o.DetID, o.BookID, o.DocDate, o.AccountID, o.Narration, o.TenantId } into g
        //                   select new
        //                   {
        //                       Voucher = g.Key.DetID,
        //                       g.Key.BookID,
        //                       g.Key.DocDate,
        //                       AccountID = g.Key.AccountID,
        //                       Narration =   g.Key.Narration,
        //                       Opening = g.Sum(x => x.Opening),
        //                       Debit  = g.Sum(x => x.Debit),
        //                       Credit = g.Sum(x => x.Credit),
        //                       g.Key.TenantId
        //                   };

        //    var countnewq = newquery.Count();

        //    var cashBook = from o in newquery
        //                   join o1 in _ChartofAccountRepository.GetAll() on new { Accid = o.AccountID, tenaid = o.TenantId } equals new { Accid = o1.Id, tenaid = o1.TenantId }
        //                   //group new { o, o1 } by new { o.AccountID, o.DetID, o.Narration, o1.AccountName } into g
        //                   select new CashBookDto
        //                       {
        //                           Voucher = o.Voucher,
        //                           BookId = o.BookID,
        //                           DocDate = o.DocDate,
        //                           AccountID = o.AccountID,
        //                           AccountName =o1.AccountName,
        //                           Narration = o.Narration,
        //                           Opening = (decimal)o.Opening,
        //                           Debit = (decimal)o.Debit, //(decimal)o.Debit,
        //                           Credit = (decimal)o.Credit, //(decimal)o.Credit
        //                           Balance = 0

        //                   };
        //    var date = DateTime.Now;
        //    decimal balance = 0;
        //    foreach (var item in cashBook)
        //    {


        //        //if (i == 0)
        //        //{
        //        //    balance += (decimal)OpeningBal.Sum() + (decimal)item.VenderActivity.Debitwal - (decimal)item.VenderActivity.Credit;
        //        //    item.VenderActivity.RunningTotal = (double)balance;
        //        //}
        //        //else
        //        //{
        //        balance += item.Debit - item.Credit;
        //        item.Balance = balance;
        //        //}


        //        //item.OpeningBalance += (decimal)OpeningBal.Sum();
        //        //item.OutstandingBalance += (decimal)Outstandbal.Sum();

        //        //i++;
        //    }

        //    string repJsonP = JsonConvert.SerializeObject(cashBook.ToList());
        //    var totalCount = cashBook.ToList();

        //    return new List<CashBookDto>(
        //        totalCount

        //        );

        //}


    }
}
