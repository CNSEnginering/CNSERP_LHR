using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ERP.Reports.Finance
{
    public class SubledgerReportAppService : ERPReportAppServiceBase
    {
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;
        private readonly IRepository<GLTRDetail> _gltrDetailRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<GLLocation> _glLocationRepository;

        public SubledgerReportAppService(IRepository<ChartofControl, string> chartofControlRepository, IRepository<GLTRHeader> gltrHeaderRepository, IRepository<GLLocation> glLocationRepository, IRepository<GLTRDetail> gltrDetailRepository, IRepository<AccountSubLedger> accountSubLedgerRepository)
        {
            _chartofControlRepository = chartofControlRepository;
            _gltrHeaderRepository = gltrHeaderRepository;
            _gltrDetailRepository = gltrDetailRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _glLocationRepository = glLocationRepository;
        }
        public List<SubedgerDto> GetSubledger(string text, DateTime fromDate, DateTime toDate, string fromAC, string toAC, int fromSubledgerId, int toSubledgerId, int locId,
             int slType, string status, int? curRate)
        {
            var tenantId = AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<SubedgerDto> subedgerDtoList = new List<SubedgerDto>();
            using (SqlConnection cn = new SqlConnection(str))
            {
                if (text == "Summary")
                {
                    using (SqlCommand cmd = new SqlCommand("sp_LedgerSummary", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FromDate", fromDate.Date);
                        cmd.Parameters.AddWithValue("@ToDate", toDate.Date);
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);
                        //cmd.Parameters.AddWithValue("@locId", locId);
                        cmd.Parameters.AddWithValue("@FromAccountID", fromAC);
                        cmd.Parameters.AddWithValue("@ToAccountID", toAC);
                        cmd.Parameters.AddWithValue("@FromSubAccID", fromSubledgerId);
                        cmd.Parameters.AddWithValue("@ToSubAccID", toSubledgerId);
                        //cmd.Parameters.AddWithValue("@slType", slType);
                        if (status == "Approved")
                        {
                            cmd.Parameters.AddWithValue("@Approved", 1);
                            cmd.Parameters.AddWithValue("@Posted", 0);
                        }
                        else if (status == "Posted" || status == "Both")
                        {
                            cmd.Parameters.AddWithValue("@Approved", 1);
                            cmd.Parameters.AddWithValue("@Posted", 1);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Approved", 0);
                            cmd.Parameters.AddWithValue("@Posted", 0);
                        }

                        cn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            var AccId = "";
                            var SubAccId = 0;
                            while (rdr.Read())
                            {
                                SubedgerDto subedgerDto = new SubedgerDto()
                                {
                                    AccountCode = rdr["AccountCode"] is DBNull ? "" : rdr["AccountCode"].ToString(),
                                    AccountTitle = rdr["AccountTitle"] is DBNull ? "" : rdr["AccountTitle"].ToString(),
                                    //DocNo = rdr["FmtDocNo"] is DBNull ? 0 : Convert.ToInt32(rdr["FmtDocNo"]),
                                    SubledgerCode = rdr["SubledgerCode"] is DBNull ? 0 : Convert.ToInt32(rdr["SubledgerCode"]),
                                    SubledgerDesc = rdr["SubledgerDesc"] is DBNull ? "" : rdr["SubledgerDesc"].ToString(),
                                    //Narration = rdr["Narration"] is DBNull ? "" : rdr["Narration"].ToString(),
                                    //DocDate = Convert.ToDateTime(rdr["DocDate"]),
                                    //BookId = rdr["BookID"] is DBNull ? "" : rdr["BookID"].ToString(),
                                    //ConfigId = rdr["ConfigID"] is DBNull ? 0 : Convert.ToInt32(rdr["ConfigID"]),
                                    Debit = rdr["Debit"] is DBNull ? 0 : Convert.ToInt32(rdr["Debit"]) / Convert.ToDouble(curRate),
                                    Credit = rdr["Credit"] is DBNull ? 0 : Convert.ToInt32(rdr["Credit"]) / Convert.ToDouble(curRate),
                                    Amount = rdr["Amount"] is DBNull ? 0 : Convert.ToDouble(rdr["Amount"]) / Convert.ToDouble(curRate),
                                    Opening = rdr["Opening"] is DBNull ? 0 : Convert.ToDouble(rdr["Opening"]) / Convert.ToDouble(curRate),
                                    //LocId = rdr["LocID"] is DBNull ? 0 : Convert.ToInt32(rdr["LocID"]),
                                    //LocDesc = Convert.ToInt32(locId) == 0 ? "All" :
                                    //rdr["LocDesc"].ToString()

                                };
                                subedgerDtoList.Add(subedgerDto);

                            }
                        }
                    }
                }
                else
                {
                    using (SqlCommand cmd = (status == "Both" ? new SqlCommand("SP_SubledgerRpt_Both", cn) : new SqlCommand("SP_SubledgerRpt", cn)))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fromDate", fromDate.Date);
                        cmd.Parameters.AddWithValue("@toDate", toDate.Date);
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);
                        cmd.Parameters.AddWithValue("@locId", locId);
                        cmd.Parameters.AddWithValue("@fromAcc", fromAC);
                        cmd.Parameters.AddWithValue("@toAcc", toAC);
                        cmd.Parameters.AddWithValue("@fromSubledgerId", fromSubledgerId);
                        cmd.Parameters.AddWithValue("@toSubledgerId", toSubledgerId);
                        cmd.Parameters.AddWithValue("@slType", slType);
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
                            cmd.Parameters.AddWithValue("@Approved", 0);
                            cmd.Parameters.AddWithValue("@Posted", 0);
                        }

                        cn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            var AccId = "";
                            var SubAccId = 0;
                            while (rdr.Read())
                            {
                                SubedgerDto subedgerDto = new SubedgerDto()
                                {
                                    AccountCode = rdr["AccountCode"] is DBNull ? "" : rdr["AccountCode"].ToString(),
                                    AccountTitle = rdr["AccountTitle"] is DBNull ? "" : rdr["AccountTitle"].ToString(),
                                    DocNo = rdr["FmtDocNo"] is DBNull ? 0 : Convert.ToInt32(rdr["FmtDocNo"]),
                                    SubledgerCode = rdr["SubledgerCode"] is DBNull ? 0 : Convert.ToInt32(rdr["SubledgerCode"]),
                                    SubledgerDesc = rdr["SubledgerDesc"] is DBNull ? "" : rdr["SubledgerDesc"].ToString(),
                                    Narration = rdr["Narration"] is DBNull ? "" : rdr["Narration"].ToString(),
                                    DocDate = Convert.ToDateTime(rdr["DocDate"]),
                                    BookId = rdr["BookID"] is DBNull ? "" : rdr["BookID"].ToString(),
                                    ConfigId = rdr["ConfigID"] is DBNull ? 0 : Convert.ToInt32(rdr["ConfigID"]),
                                    Debit = rdr["Debit"] is DBNull ? 0 : Convert.ToInt32(rdr["Debit"]) / Convert.ToDouble(curRate),
                                    Credit = rdr["Credit"] is DBNull ? 0 : Convert.ToInt32(rdr["Credit"]) / Convert.ToDouble(curRate),
                                    Amount = rdr["Amount"] is DBNull ? 0 : Convert.ToDouble(rdr["Amount"]) / Convert.ToDouble(curRate),
                                    Opening = rdr["Opening"] is DBNull ? 0 : Convert.ToDouble(rdr["Opening"]) / Convert.ToDouble(curRate),
                                    LocId = rdr["LocID"] is DBNull ? 0 : Convert.ToInt32(rdr["LocID"]),
                                    LocDesc = Convert.ToInt32(locId) == 0 ? "All" :
                                    rdr["LocDesc"].ToString()

                                };

                                subedgerDtoList.Add(subedgerDto);


                            }
                        }
                    }
                }


                if (subedgerDtoList.Count == 0)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_OpningAccount", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fromDate", fromDate.Date);
                        cmd.Parameters.AddWithValue("@toDate", toDate.Date);
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);
                        cmd.Parameters.AddWithValue("@locId", locId);
                        cmd.Parameters.AddWithValue("@fromAcc", fromAC);
                        cmd.Parameters.AddWithValue("@toAcc", toAC);
                        cmd.Parameters.AddWithValue("@fromSubledgerId", fromSubledgerId);
                        cmd.Parameters.AddWithValue("@toSubledgerId", toSubledgerId);
                        cmd.Parameters.AddWithValue("@slType", slType);
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
                            cmd.Parameters.AddWithValue("@Approved", 0);
                            cmd.Parameters.AddWithValue("@Posted", 0);
                        }


                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                SubedgerDto subedgerDto = new SubedgerDto()
                                {
                                    AccountCode = rdr["AccountID"] is DBNull ? "" : rdr["AccountID"].ToString(),
                                    AccountTitle = rdr["AccountName"] is DBNull ? "" : rdr["AccountName"].ToString(),
                                    // DocNo = rdr["FmtDocNo"] is DBNull ? 0 : Convert.ToInt32(rdr["FmtDocNo"]),
                                    SubledgerCode = rdr["SubAccID"] is DBNull ? 0 : Convert.ToInt32(rdr["SubAccID"]),
                                    SubledgerDesc = rdr["SubAccName"] is DBNull ? "" : rdr["SubAccName"].ToString(),
                                    //Narration = rdr["Narration"] is DBNull ? "" : rdr["Narration"].ToString(),
                                    DocDate = fromDate.Date,
                                    //BookId = rdr["BookID"] is DBNull ? "" : rdr["BookID"].ToString(),
                                    //ConfigId = rdr["ConfigID"] is DBNull ? 0 : Convert.ToInt32(rdr["ConfigID"]),
                                    //Debit = rdr["Debit"] is DBNull ? 0 : Convert.ToInt32(rdr["Debit"]) / Convert.ToDouble(curRate),
                                    //Credit = rdr["Credit"] is DBNull ? 0 : Convert.ToInt32(rdr["Credit"]) / Convert.ToDouble(curRate),
                                    //Amount = rdr["Amount"] is DBNull ? 0 : Convert.ToDouble(rdr["Amount"]) / Convert.ToDouble(curRate),
                                    Opening = rdr["Opening"] is DBNull ? 0 : Convert.ToDouble(rdr["Opening"]) / Convert.ToDouble(curRate),
                                    //LocId = rdr["LocID"] is DBNull ? 0 : Convert.ToInt32(rdr["LocID"]),
                                    //LocDesc = Convert.ToInt32(locId) == 0 ? "All" :
                                    //rdr["LocDesc"].ToString()
                                };

                                subedgerDtoList.Add(subedgerDto);
                            }
                        }
                    }

                }
                // cn.Close();

            }
            return subedgerDtoList;
            //IQueryable<GLTRHeader> gltrHeader;

            //if (status == "Approved")
            //{
            //    gltrHeader = _gltrHeaderRepository.GetAll()
            //   .Where(e => e.TenantId == tenantId && e.Approved == true && e.Posted == false && e.DocDate.Date >= fromDate.Date && e.DocDate.Date <= toDate.Date);
            //}
            //else if (status == "Posted")
            //{
            //    gltrHeader = _gltrHeaderRepository.GetAll()
            //   .Where(e => e.TenantId == tenantId && e.Approved == false && e.Posted == true && e.DocDate.Date >= fromDate.Date && e.DocDate.Date <= toDate.Date);
            //}
            //else
            //{
            //    gltrHeader = _gltrHeaderRepository.GetAll()
            //   .Where(e => e.TenantId == tenantId && e.Approved == true && e.Posted == true && e.DocDate.Date >= fromDate.Date && e.DocDate.Date <= toDate.Date);
            //}

            //var gltrDetail = _gltrDetailRepository.GetAll().Where(e => e.TenantId == tenantId).Where(d => d.AccountID.CompareTo(fromAC) >= 0 && d.AccountID.CompareTo(toAC) <= 0);
            //IQueryable<ChartofControl> chartOfAc;
            //if (slType != 0)
            //{
            //    chartOfAc = _chartofControlRepository.GetAll().Where(e => e.TenantId == tenantId && e.SubLedger == true && e.SLType == slType);
            //}
            //else
            //{
            //    chartOfAc = _chartofControlRepository.GetAll().Where(e => e.TenantId == tenantId && e.SubLedger == true);
            //}


            //var subLedger = _accountSubLedgerRepository.GetAll().Where(e => e.TenantId == tenantId).Where(d => d.Id.CompareTo(fromSubledgerId) >= 0 && d.Id.CompareTo(toSubledgerId) <= 0);
            //IQueryable<GLLocation> locations;
            //if (locId != 0)
            //{
            //    locations = _glLocationRepository.GetAll().Where(o => o.TenantId == tenantId && o.LocId == locId);
            //}
            //else
            //{
            //    locations = _glLocationRepository.GetAll().Where(o => o.TenantId == tenantId);
            //}


            //var result = from h in gltrHeader
            //             join d in gltrDetail on new { X = h.Id, Y = h.TenantId } equals new { X = d.DetID, Y = d.TenantId }
            //             join ca in chartOfAc on new { x = d.AccountID, y = d.TenantId } equals new { x = ca.Id, y = ca.TenantId }
            //             join l in locations on new { x = h.LocId, y = h.TenantId } equals new { x = l.LocId, y = (int)l.TenantId }
            //             join sl in subLedger on new { X = d.AccountID, Y = (int)d.SubAccID, z = (int)d.TenantId } equals new { X = sl.AccountID, Y = sl.Id, z = sl.TenantId }
            //             into sb
            //             from p in sb.DefaultIfEmpty()

            //             select new
            //             {
            //                 AccountCode = ca.Id,
            //                 AccountTitle = ca.AccountName,
            //                 DocNo = h.DocNo,
            //                 //SubledgerCode = d.SubAccID == null ? 0 : (int)d.SubAccID,
            //                 //SubledgerDesc = sb == null ? "" : p.SubAccName,
            //                 //Narration = d.Narration,
            //                 //DocDate = h.DocDate,
            //                 //BookId = h.BookID,
            //                 //ConfigId = h.ConfigID,
            //                 //Debit = (double)(d.Amount > 0 ? d.Amount : 0),
            //                 //Credit = (double)(d.Amount < 0 ? -(d.Amount) : 0),
            //                 //Amount = (double)(d.Amount),
            //                 //LocId = h.LocId,
            //                 //LocDesc = l.LocDesc
            //             };


            //var generalLedg = result.ToList();
            //var generalLedger = (from ca in chartOfAc
            //                     join d in gltrDetail on ca.Id equals d.AccountID
            //                     join h in gltrHeader on d.DetID equals h.Id
            //                     join l in locations on h.LocId equals l.LocId
            //                     join sl in subLedger on new { X = d.AccountID, Y = (int)d.SubAccID } equals new { X = sl.AccountID, Y = sl.Id } //into sb
            //                                                                                                                                     // from p in sb.DefaultIfEmpty()
            //                                                                                                                                     //orderby h.DocDate ascending
            //                     select new SubedgerDto
            //                     {
            //                         AccountCode = ca.Id,
            //                         AccountTitle = ca.AccountName,
            //                         DocNo = h.DocNo,
            //                         SubledgerCode = d.SubAccID == null ? 0 : (int)d.SubAccID,
            //                         SubledgerDesc = sl.SubAccName == null ? "" : sl.SubAccName,
            //                         Narration = d.Narration,
            //                         DocDate = h.DocDate,
            //                         BookId = h.BookID,
            //                         ConfigId = h.ConfigID,
            //                         Debit = (double)(d.Amount > 0 ? d.Amount : 0),
            //                         Credit = (double)(d.Amount < 0 ? -(d.Amount) : 0),
            //                         Amount = (double)(d.Amount),
            //                         LocId = h.LocId,
            //                         LocDesc = l.LocDesc
            //                     }).ToList();

            //return generalLedger;
        }

        //public List<SubedgerDto> GetSubledger(string text,DateTime fromDate, DateTime toDate, string fromAC, string toAC, int fromSubledgerId, int toSubledgerId, int locId,
        //    int slType, string status, int? curRate)
        //{
        //    var tenantId = AbpSession.TenantId;
        //    string str = ConfigurationManager.AppSettings["ConnectionString"];
        //    List<SubedgerDto> subedgerDtoList = new List<SubedgerDto>();
        //    using (SqlConnection cn = new SqlConnection(str))
        //    {


        //        using (SqlCommand cmd = (status == "Both" ? new SqlCommand("SP_SubledgerRpt_Both", cn) : new SqlCommand("SP_SubledgerRpt", cn)))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@fromDate", fromDate.Date);
        //            cmd.Parameters.AddWithValue("@toDate", toDate.Date);
        //            cmd.Parameters.AddWithValue("@tenantId", tenantId);
        //            cmd.Parameters.AddWithValue("@locId", locId);
        //            cmd.Parameters.AddWithValue("@fromAcc", fromAC);
        //            cmd.Parameters.AddWithValue("@toAcc", toAC);
        //            cmd.Parameters.AddWithValue("@fromSubledgerId", fromSubledgerId);
        //            cmd.Parameters.AddWithValue("@toSubledgerId", toSubledgerId);
        //            cmd.Parameters.AddWithValue("@slType", slType);
        //            if (status == "Approved")
        //            {
        //                cmd.Parameters.AddWithValue("@Approved", 1);
        //                cmd.Parameters.AddWithValue("@Posted", 0);
        //            }
        //            else if (status == "Posted")
        //            {
        //                cmd.Parameters.AddWithValue("@Approved", 1);
        //                cmd.Parameters.AddWithValue("@Posted", 1);
        //            }
        //            else
        //            {
        //                cmd.Parameters.AddWithValue("@Approved", 0);
        //                cmd.Parameters.AddWithValue("@Posted", 0);
        //            }

        //            cn.Open();
        //            using (SqlDataReader rdr = cmd.ExecuteReader())
        //            {
        //                while (rdr.Read())
        //                {
        //                    SubedgerDto subedgerDto = new SubedgerDto()
        //                    {
        //                        AccountCode = rdr["AccountCode"] is DBNull ? "" : rdr["AccountCode"].ToString(),
        //                        AccountTitle = rdr["AccountTitle"] is DBNull ? "" : rdr["AccountTitle"].ToString(),
        //                        DocNo = rdr["FmtDocNo"] is DBNull ? 0 : Convert.ToInt32(rdr["FmtDocNo"]),
        //                        SubledgerCode = rdr["SubledgerCode"] is DBNull ? 0 : Convert.ToInt32(rdr["SubledgerCode"]),
        //                        SubledgerDesc = rdr["SubledgerDesc"] is DBNull ? "" : rdr["SubledgerDesc"].ToString(),
        //                        Narration = rdr["Narration"] is DBNull ? "" : rdr["Narration"].ToString(),
        //                        DocDate = Convert.ToDateTime(rdr["DocDate"]),
        //                        BookId = rdr["BookID"] is DBNull ? "" : rdr["BookID"].ToString(),
        //                        ConfigId = rdr["ConfigID"] is DBNull ? 0 : Convert.ToInt32(rdr["ConfigID"]),
        //                        Debit = rdr["Debit"] is DBNull ? 0 : Convert.ToInt32(rdr["Debit"]) / Convert.ToDouble(curRate),
        //                        Credit = rdr["Credit"] is DBNull ? 0 : Convert.ToInt32(rdr["Credit"]) / Convert.ToDouble(curRate),
        //                        Amount = rdr["Amount"] is DBNull ? 0 : Convert.ToDouble(rdr["Amount"]) / Convert.ToDouble(curRate),
        //                        Opening = rdr["Opening"] is DBNull ? 0 : Convert.ToDouble(rdr["Opening"]) / Convert.ToDouble(curRate),
        //                        LocId = rdr["LocID"] is DBNull ? 0 : Convert.ToInt32(rdr["LocID"]),
        //                        LocDesc = Convert.ToInt32(locId) == 0 ? "All" :
        //                        rdr["LocDesc"].ToString()
        //                    };

        //                    subedgerDtoList.Add(subedgerDto);
        //                }
        //            }
        //        }
        //        // cn.Close();

        //    }
        //    return subedgerDtoList;
        //    //IQueryable<GLTRHeader> gltrHeader;

        //    //if (status == "Approved")
        //    //{
        //    //    gltrHeader = _gltrHeaderRepository.GetAll()
        //    //   .Where(e => e.TenantId == tenantId && e.Approved == true && e.Posted == false && e.DocDate.Date >= fromDate.Date && e.DocDate.Date <= toDate.Date);
        //    //}
        //    //else if (status == "Posted")
        //    //{
        //    //    gltrHeader = _gltrHeaderRepository.GetAll()
        //    //   .Where(e => e.TenantId == tenantId && e.Approved == false && e.Posted == true && e.DocDate.Date >= fromDate.Date && e.DocDate.Date <= toDate.Date);
        //    //}
        //    //else
        //    //{
        //    //    gltrHeader = _gltrHeaderRepository.GetAll()
        //    //   .Where(e => e.TenantId == tenantId && e.Approved == true && e.Posted == true && e.DocDate.Date >= fromDate.Date && e.DocDate.Date <= toDate.Date);
        //    //}

        //    //var gltrDetail = _gltrDetailRepository.GetAll().Where(e => e.TenantId == tenantId).Where(d => d.AccountID.CompareTo(fromAC) >= 0 && d.AccountID.CompareTo(toAC) <= 0);
        //    //IQueryable<ChartofControl> chartOfAc;
        //    //if (slType != 0)
        //    //{
        //    //    chartOfAc = _chartofControlRepository.GetAll().Where(e => e.TenantId == tenantId && e.SubLedger == true && e.SLType == slType);
        //    //}
        //    //else
        //    //{
        //    //    chartOfAc = _chartofControlRepository.GetAll().Where(e => e.TenantId == tenantId && e.SubLedger == true);
        //    //}


        //    //var subLedger = _accountSubLedgerRepository.GetAll().Where(e => e.TenantId == tenantId).Where(d => d.Id.CompareTo(fromSubledgerId) >= 0 && d.Id.CompareTo(toSubledgerId) <= 0);
        //    //IQueryable<GLLocation> locations;
        //    //if (locId != 0)
        //    //{
        //    //    locations = _glLocationRepository.GetAll().Where(o => o.TenantId == tenantId && o.LocId == locId);
        //    //}
        //    //else
        //    //{
        //    //    locations = _glLocationRepository.GetAll().Where(o => o.TenantId == tenantId);
        //    //}


        //    //var result = from h in gltrHeader
        //    //             join d in gltrDetail on new { X = h.Id, Y = h.TenantId } equals new { X = d.DetID, Y = d.TenantId }
        //    //             join ca in chartOfAc on new { x = d.AccountID, y = d.TenantId } equals new { x = ca.Id, y = ca.TenantId }
        //    //             join l in locations on new { x = h.LocId, y = h.TenantId } equals new { x = l.LocId, y = (int)l.TenantId }
        //    //             join sl in subLedger on new { X = d.AccountID, Y = (int)d.SubAccID, z = (int)d.TenantId } equals new { X = sl.AccountID, Y = sl.Id, z = sl.TenantId }
        //    //             into sb
        //    //             from p in sb.DefaultIfEmpty()

        //    //             select new
        //    //             {
        //    //                 AccountCode = ca.Id,
        //    //                 AccountTitle = ca.AccountName,
        //    //                 DocNo = h.DocNo,
        //    //                 //SubledgerCode = d.SubAccID == null ? 0 : (int)d.SubAccID,
        //    //                 //SubledgerDesc = sb == null ? "" : p.SubAccName,
        //    //                 //Narration = d.Narration,
        //    //                 //DocDate = h.DocDate,
        //    //                 //BookId = h.BookID,
        //    //                 //ConfigId = h.ConfigID,
        //    //                 //Debit = (double)(d.Amount > 0 ? d.Amount : 0),
        //    //                 //Credit = (double)(d.Amount < 0 ? -(d.Amount) : 0),
        //    //                 //Amount = (double)(d.Amount),
        //    //                 //LocId = h.LocId,
        //    //                 //LocDesc = l.LocDesc
        //    //             };


        //    //var generalLedg = result.ToList();
        //    //var generalLedger = (from ca in chartOfAc
        //    //                     join d in gltrDetail on ca.Id equals d.AccountID
        //    //                     join h in gltrHeader on d.DetID equals h.Id
        //    //                     join l in locations on h.LocId equals l.LocId
        //    //                     join sl in subLedger on new { X = d.AccountID, Y = (int)d.SubAccID } equals new { X = sl.AccountID, Y = sl.Id } //into sb
        //    //                                                                                                                                     // from p in sb.DefaultIfEmpty()
        //    //                                                                                                                                     //orderby h.DocDate ascending
        //    //                     select new SubedgerDto
        //    //                     {
        //    //                         AccountCode = ca.Id,
        //    //                         AccountTitle = ca.AccountName,
        //    //                         DocNo = h.DocNo,
        //    //                         SubledgerCode = d.SubAccID == null ? 0 : (int)d.SubAccID,
        //    //                         SubledgerDesc = sl.SubAccName == null ? "" : sl.SubAccName,
        //    //                         Narration = d.Narration,
        //    //                         DocDate = h.DocDate,
        //    //                         BookId = h.BookID,
        //    //                         ConfigId = h.ConfigID,
        //    //                         Debit = (double)(d.Amount > 0 ? d.Amount : 0),
        //    //                         Credit = (double)(d.Amount < 0 ? -(d.Amount) : 0),
        //    //                         Amount = (double)(d.Amount),
        //    //                         LocId = h.LocId,
        //    //                         LocDesc = l.LocDesc
        //    //                     }).ToList();

        //    //return generalLedger;
        //}

        public class SubedgerDto
        {
            public string AccountCode { get; set; }

            public string AccountTitle { get; set; }

            public int DocNo { get; set; }

            public int SubledgerCode { get; set; }

            public string SubledgerDesc { get; set; }

            public string Narration { get; set; }

            public DateTime DocDate { get; set; }

            public string BookId { get; set; }

            public int ConfigId { get; set; }

            public double Debit { get; set; }

            public double Credit { get; set; }

            public double Amount { get; set; }

            public double Opening { get; set; }

            public int LocId { get; set; }

            public string LocDesc { get; set; }
        }
    }
}
