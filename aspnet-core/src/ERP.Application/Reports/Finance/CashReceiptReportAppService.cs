using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.SetupForms.LedgerType;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using ERP.Reports.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ERP.Reports.Finance
{
    public class CashReceiptReportAppService : ERPReportAppServiceBase, ICashReceiptReportAppService
    {
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;
        private readonly IRepository<GLTRDetail> _gltrDetailRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<GLBOOKS> _glbooksRepository;
        private readonly IRepository<GLOption> _glOptionRepository;
        private readonly IRepository<GLLocation> _glLocationRepository;
        private readonly IRepository<LedgerType> _ledgerTypeRepository;
        private readonly IRepository<CurrencyRate, string> _currencyRepository;

        public CashReceiptReportAppService(IRepository<GLOption> glOptionRepository,
            IRepository<GLBOOKS> glbooksRepository, IRepository<GLLocation> glLocationRepository,
            IRepository<ChartofControl, string> chartofControlRepository, IRepository<GLTRHeader> gltrHeaderRepository,
            IRepository<GLTRDetail> gltrDetailRepository, IRepository<AccountSubLedger> accountSubLedgerRepository, IRepository<CurrencyRate, string> currencyRepository,
            IRepository<LedgerType> ledgerTypeRepository)
        {
            _chartofControlRepository = chartofControlRepository;
            _gltrHeaderRepository = gltrHeaderRepository;
            _gltrDetailRepository = gltrDetailRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _glbooksRepository = glbooksRepository;
            _glOptionRepository = glOptionRepository;
            _glLocationRepository = glLocationRepository;
            _ledgerTypeRepository = ledgerTypeRepository;
            _currencyRepository = currencyRepository;
        }



        public GLOption GetSignatures(int tenantId)
        {
            return _glOptionRepository.GetAll().Where(o => o.TenantId == tenantId).First();
        }

        public List<CashReceiptModel> GetCashReceipt(int? tenantId, string bookId, int? year, int? month, int fromConfigId, int toConfigId, int fromDoc,
            int toDoc, int locId, string curId, double? curRate, string status)
        {
            if (tenantId == null)
            {
                tenantId = AbpSession.TenantId;
            }

            year = (year == null ? DateTime.Now.Year : year);
            month = (month == null ? DateTime.Now.Month : month);

            //IEnumerable<GLTRHeader> gltrHeader = null;


            var curNarration = _currencyRepository.FirstOrDefault(x => x.TenantId == tenantId && x.Id == curId).Narration;
            var curUnit = _currencyRepository.FirstOrDefault(x => x.TenantId == tenantId && x.Id == curId).Unit;


            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<CashReceiptModel> cashReceiptModelDtoList = new List<CashReceiptModel>();
            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Vocuher", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@month", month);
                    cmd.Parameters.AddWithValue("@tenantId", tenantId);
                    cmd.Parameters.AddWithValue("@fromConfigId", fromConfigId);
                    cmd.Parameters.AddWithValue("@toConfigId", toConfigId);
                    cmd.Parameters.AddWithValue("@fromDoc", fromDoc);
                    cmd.Parameters.AddWithValue("@toDoc", toDoc);
                    cmd.Parameters.AddWithValue("@bookId", bookId);

                    if (status == "Approved")
                    {
                        cmd.Parameters.AddWithValue("@approved", "true");
                        cmd.Parameters.AddWithValue("@posted", "false");
                        // gltrHeader = _gltrHeaderRepository.GetAll()
                        //.Where(e => e.TenantId == tenantId)
                        //.WhereIf(month != null, e => e.DocMonth == month)
                        //.WhereIf(year != null, e => e.DocDate.Year == year)
                        //.WhereIf(bookId != "All", x => x.BookID == bookId)
                        //.Where(h => h.FmtDocNo.CompareTo(fromDoc) >= 0 && h.FmtDocNo.CompareTo(toDoc) <= 0)
                        //.Where(h => h.ConfigID.CompareTo(fromConfigId) >= 0 && h.ConfigID.CompareTo(toConfigId) <= 0)
                        //.Where(h => h.Approved == true && h.Posted == false);
                    }
                    else if (status == "Posted")
                    {
                        cmd.Parameters.AddWithValue("@approved", "true");
                        cmd.Parameters.AddWithValue("@posted", "true");
                        // gltrHeader = _gltrHeaderRepository.GetAll()
                        //.Where(e => e.TenantId == tenantId)
                        //.WhereIf(month != null, e => e.DocMonth == month)
                        //.WhereIf(year != null, e => e.DocDate.Year == year)
                        //.WhereIf(bookId != "All", x => x.BookID == bookId)
                        //.Where(h => h.FmtDocNo.CompareTo(fromDoc) >= 0 && h.FmtDocNo.CompareTo(toDoc) <= 0)
                        //.Where(h => h.ConfigID.CompareTo(fromConfigId) >= 0 && h.ConfigID.CompareTo(toConfigId) <= 0)
                        //.Where(h => h.Approved == true && h.Posted == true);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@approved", "false");
                        cmd.Parameters.AddWithValue("@posted", "false");
                        // gltrHeader = _gltrHeaderRepository.GetAll()
                        //.Where(e => e.TenantId == tenantId)
                        //.WhereIf(month != null, e => e.DocMonth == month)
                        //.WhereIf(year != null, e => e.DocDate.Year == year)
                        //.WhereIf(bookId != "All", x => x.BookID == bookId)
                        //.Where(h => h.FmtDocNo.CompareTo(fromDoc) >= 0 && h.FmtDocNo.CompareTo(toDoc) <= 0)
                        //.Where(h => h.ConfigID.CompareTo(fromConfigId) >= 0 && h.ConfigID.CompareTo(toConfigId) <= 0);
                    }


                    cn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            CashReceiptModel cashReceiptModelDto = new CashReceiptModel()
                            {
                                DetID = rdr["DetID"] is DBNull ? 0 : Convert.ToInt32(rdr["DetID"]),
                                DetailID = rdr["DetailID"] is DBNull ? 0 : Convert.ToInt32(rdr["DetailID"]),
                                Integrated = rdr["Integrated"] is DBNull ? false : Convert.ToBoolean(rdr["Integrated"]),
                                Reference = rdr["Reference"] is DBNull ? "" : rdr["Reference"].ToString(),
                                FmtDocNo = rdr["FmtDocNo"] is DBNull ? 0 : Convert.ToInt32(rdr["FmtDocNo"]),
                                DocNo = rdr["DocNo"] is DBNull ? 0 : Convert.ToInt32(rdr["DocNo"]),
                                Posted = rdr["Posted"] is DBNull ? false : Convert.ToBoolean(rdr["Posted"]),
                                PostedBy = rdr["PostedBy"] is DBNull ? "" : rdr["PostedBy"].ToString(),
                                ApprovedBy = rdr["PostedBy"] is DBNull ? "" : rdr["PostedBy"].ToString(),
                                NarrationD = rdr["NarrationD"] is DBNull ? "" : rdr["NarrationD"].ToString(),
                                CreatedBy = rdr["CreatedBy"] is DBNull ? "" : rdr["CreatedBy"].ToString(),
                                CreatedAt = rdr["CreatedOn"] is DBNull ? new DateTime() : Convert.ToDateTime(rdr["CreatedOn"]),
                                DocDate = rdr["DocDate"] is DBNull ? new DateTime() : Convert.ToDateTime(rdr["DocDate"]),
                                ConfigId = rdr["ConfigId"] is DBNull ? 0 : Convert.ToInt32(rdr["ConfigId"]),
                                BookId = rdr["BookId"] is DBNull ? "" : rdr["BookId"].ToString(),
                                BookName = rdr["BookName"] is DBNull ? "" : rdr["BookName"].ToString(),
                                LocId = rdr["LocId"] is DBNull ? 0 : Convert.ToInt32(rdr["LocId"]),
                                LocDesc = rdr["LocDesc"] is DBNull ? "" : rdr["LocDesc"].ToString(),
                                AccountCode = rdr["AccountID"] is DBNull ? "" : rdr["AccountID"].ToString(),
                                AccountTitle = rdr["AccountName"] is DBNull ? "" : rdr["AccountName"].ToString(),
                                SubledgerCode = rdr["SubAccID"] is DBNull ? 0 : Convert.ToInt32(rdr["SubAccID"]),
                                SubledgerDesc = rdr["SubAccName"] is DBNull ? "" : rdr["SubAccName"].ToString(),
                                // DetailNarration = rdr["Narration"].ToString(),
                                //Debit = rdr["Amount"] is DBNull ? 0 : Math.Round((double)((Convert.ToInt32(rdr["Amount"]) > 0 ? Convert.ToInt32(rdr["Amount"]) : 0) / curRate), 2),
                                //Credit = rdr["Amount"] is DBNull ? 0 : Math.Round((double)((Convert.ToInt32(rdr["Amount"]) < 0 ? -(Convert.ToInt32(rdr["Amount"])) : 0) / curRate), 2),
                                //Amount = rdr["Amount"] is DBNull ? 0 : Math.Round((double)((Convert.ToInt32(rdr["Amount"])) / curRate), 2),

                                Debit = rdr["Amount"] is DBNull ? 0 : Math.Round((double)((Convert.ToInt32(rdr["Amount"]) > 0 ? Convert.ToDouble(rdr["Amount"]) : 0) / curRate), 2),
                                Credit = rdr["Amount"] is DBNull ? 0 : Math.Round((double)((Convert.ToInt32(rdr["Amount"]) < 0 ? -(Convert.ToDouble(rdr["Amount"])) : 0) / curRate), 2),
                                Amount = rdr["Amount"] is DBNull ? 0 : Math.Round((double)((Convert.ToDouble(rdr["Amount"])) / curRate), 2),

                                IsAuto = rdr["IsAuto"] is DBNull ? false : Convert.ToBoolean(rdr["IsAuto"]),
                                FirstSignature = rdr["FirstSignature"].ToString(),
                                SecondSignature = rdr["SecondSignature"].ToString(),
                                ThirdSignature = rdr["ThirdSignature"].ToString(),
                                FourthSignature = rdr["FourthSignature"].ToString(),
                                FifthSignature = rdr["FifthSignature"].ToString(),
                                SixthSignature = rdr["SixthSignature"].ToString(),
                                ChequeNo = rdr["ChNumber"] is DBNull ? "" : rdr["ChNumber"].ToString(),
                                //ChequeType = h.ChType == null ? "" : (h.ChType == 1 ? "Cash" : "Cross"),
                                ChequeType = rdr["ChType"] is DBNull ? "" : rdr["ChType"].ToString(),
                                CurId = curId,
                                CurRate = curRate,
                                UserId = rdr["CreatedBy"] is DBNull ? "" : rdr["CreatedBy"].ToString(),
                                LedgerTypeName = rdr["LedgerDesc"] is DBNull ? "" : rdr["LedgerDesc"].ToString(),
                                //(
                                //_ledgerTypeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LedgerID == p.SLType).Count() > 0 ?
                                //_ledgerTypeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LedgerID == p.SLType).First().LedgerDesc
                                //: "") :
                                //"",
                                Narration = rdr["Narration"] is DBNull ? "" : rdr["Narration"].ToString(),
                                RefType = (rdr["BookId"].ToString() == "CR" || rdr["BookId"].ToString() == "BR") ? "Received From:" : "Payee:",
                                CurNarration = curNarration,
                                CurUnit = curUnit
                            };
                            cashReceiptModelDtoList.Add(cashReceiptModelDto);
                        }
                    }
                }
                // cn.Close();
            }
            return cashReceiptModelDtoList;

            //var gltrDetail = _gltrDetailRepository.GetAll().Where(e => e.TenantId == tenantId);
            //var chartOfAc = _chartofControlRepository.GetAll().Where(e => e.TenantId == tenantId);
            //var subLedger = _accountSubLedgerRepository.GetAll().Where(e => e.TenantId == tenantId);
            //var books = _glbooksRepository.GetAll().Where(e => e.TenantId == tenantId);
            //var glSetup = _glOptionRepository.FirstOrDefault(o => o.TenantId == tenantId);
            //var locations = _glLocationRepository.GetAll().Where(o => o.TenantId == tenantId)
            //        .WhereIf(locId != 0, x => x.LocId == locId);





            //var test = from ca in chartOfAc
            //           join d in gltrDetail on ca.Id equals d.AccountID
            //           join h in gltrHeader on d.DetID equals h.Id
            //           join b in books on h.BookID equals b.BookID
            //           //join l in locations on h.LocId equals l.LocId
            //           join sl in subLedger on new { X = d.AccountID, Y = (int)d.SubAccID } equals new { X = sl.AccountID, Y = sl.Id } into sb
            //           from s in sb.DefaultIfEmpty()
            //           orderby h.DocDate
            //           select new CashReceiptModel
            //           {
            //               AccountCode = ca.Id,
            //               //AccountTitle = ca.AccountName,
            //               //DocNo = h.DocNo,
            //               //Posted = h.Posted,
            //               //SubledgerCode = (int)d.SubAccID,
            //               //SubledgerDesc = s.SubAccName != null ? s.SubAccName : "",
            //               //Narration = h.NARRATION,
            //               //DetailNarration = d.Narration,
            //               //DocDate = h.DocDate,
            //               //BookId = h.BookID,
            //               //BookName = b.BookName,
            //               //ConfigId = h.ConfigID,
            //               //Debit = (double)(d.Amount > 0 ? d.Amount : 0),
            //               //Credit = (double)(d.Amount < 0 ? -(d.Amount) : 0),
            //               //Amount = (double)(d.Amount),
            //               //ApprovedBy = h.AprovedBy != null ? h.AprovedBy : "",
            //               //PostedBy = h.PostedBy != null ? h.PostedBy : "",
            //               //LocId = h.LocId,
            //               //LocDesc = l.LocDesc,
            //               //IsAuto = d.IsAuto,
            //               //FirstSignature = glSetup.FirstSignature != null ? glSetup.FirstSignature : "",
            //               //SecondSignature = glSetup.SecondSignature != null ? glSetup.SecondSignature : "",
            //               //ThirdSignature = glSetup.ThirdSignature != null ? glSetup.ThirdSignature : "",
            //               //FourthSignature = glSetup.FourthSignature != null ? glSetup.FourthSignature : "",
            //               //FifthSignature = glSetup.FifthSignature != null ? glSetup.FifthSignature : "",
            //               //SixthSignature = glSetup.SixthSignature != null ? glSetup.SixthSignature : "",
            //               //CreatedBy = h.CreatedBy != null ? h.CreatedBy : "",
            //               //CreatedAt = Convert.ToDateTime(h.CreatedOn)
            //           };

            ///  change by Waleed

            //var cashReceipt = (from h in gltrHeader
            //                   join loc in locations on new { h.LocId, h.TenantId } equals new { loc.LocId, loc.TenantId }
            //                   join book in _glbooksRepository.GetAll() on new { h.BookID, h.TenantId } equals new { book.BookID, book.TenantId }
            //                   join d in _gltrDetailRepository.GetAll() on new { x1 = h.Id, h.TenantId } equals new { x1 = d.DetID, d.TenantId }
            //                   join acc in _chartofControlRepository.GetAll() on new { x1 = d.AccountID, d.TenantId } equals new { x1 = acc.Id, acc.TenantId }
            //                   join sub in _accountSubLedgerRepository.GetAll() on new { AccountID = d.AccountID, d.SubAccID, d.TenantId } equals new { sub.AccountID, SubAccID = (int?)sub.Id, sub.TenantId } into ps
            //                   from p in ps.DefaultIfEmpty()
            //                       //join ledg in _ledgerTypeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
            //                       //on new { A = Convert.ToInt32(p.SLType), B = p.TenantId } equals new { A = ledg.LedgerID, B = ledg.TenantId }
            //                   orderby d.Amount descending
            //                   // orderby h.DocDate.ToShortDateString()
            //                   select new CashReceiptModel
            //                   {
            //                       Integrated = book.Integrated,
            //                       Reference = h.Reference,
            //                       FmtDocNo = h.FmtDocNo,
            //                       DocNo = h.DocNo,
            //                       Posted = h.Posted,
            //                       PostedBy = h.PostedBy,
            //                       ApprovedBy = h.AprovedBy,
            //                       NarrationD = d.Narration,
            //                       CreatedBy = h.CreatedBy,
            //                       CreatedAt = Convert.ToDateTime(h.CreatedOn),
            //                       DocDate = h.DocDate,
            //                       ConfigId = h.ConfigID,
            //                       BookId = h.BookID,
            //                       BookName = book.BookName,
            //                       LocId = h.LocId,
            //                       LocDesc = loc.LocDesc,
            //                       AccountCode = d.AccountID,
            //                       AccountTitle = acc.AccountName,
            //                       SubledgerCode = p == null ? 0 : p.Id,
            //                       SubledgerDesc = p == null ? "" : p.SubAccName,
            //                       DetailNarration = d.Narration,
            //                       Debit = Math.Round((double)((d.Amount > 0 ? d.Amount : 0) / curRate), 2),
            //                       Credit = Math.Round((double)((d.Amount < 0 ? -(d.Amount) : 0) / curRate), 2),
            //                       Amount = Math.Round((double)((d.Amount) / curRate), 2),
            //                       IsAuto = d.IsAuto,
            //                       FirstSignature = glSetup.FirstSignature != null ? glSetup.FirstSignature : "",
            //                       SecondSignature = glSetup.SecondSignature != null ? glSetup.SecondSignature : "",
            //                       ThirdSignature = glSetup.ThirdSignature != null ? glSetup.ThirdSignature : "",
            //                       FourthSignature = glSetup.FourthSignature != null ? glSetup.FourthSignature : "",
            //                       FifthSignature = glSetup.FifthSignature != null ? glSetup.FifthSignature : "",
            //                       SixthSignature = glSetup.SixthSignature != null ? glSetup.SixthSignature : "",
            //                       ChequeNo = h.ChNumber,
            //                       //ChequeType = h.ChType == null ? "" : (h.ChType == 1 ? "Cash" : "Cross"),
            //                       ChequeType = h.ChType.ToString(),
            //                       CurId = curId,
            //                       CurRate = curRate,
            //                       UserId = h.CreatedBy,
            //                       LedgerTypeName = (p != null) ?
            //                       (
            //                       _ledgerTypeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LedgerID == p.SLType).Count() > 0 ?
            //                       _ledgerTypeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LedgerID == p.SLType).First().LedgerDesc
            //                       : "") :
            //                       "",
            //                       Narration = h.NARRATION,
            //                       RefType = (h.BookID == "CR" || h.BookID == "BR") ? "Received From:" : "Payee:"
            //                   });


            //string pjson = JsonConvert.SerializeObject(cashReceipt);

            // return cashReceipt.ToList();


        }

        public double? GetDebitSum(int tenantId, int docNo, string bookId, int docMonth)
        {
            return (from a in _gltrHeaderRepository.GetAll()
                    join
                    b in _gltrDetailRepository.GetAll()
                    on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                    where (a.TenantId == tenantId && a.DocNo == docNo && b.Amount > 0 && a.BookID == bookId && a.DocMonth == docMonth)
                    select (b.Amount)).Sum();
        }

    }

}

