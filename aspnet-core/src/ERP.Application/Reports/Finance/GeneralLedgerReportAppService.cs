using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
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
    public class GeneralLedgerReportAppService : ERPReportAppServiceBase
    {
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;
        private readonly IRepository<GLTRDetail> _gltrDetailRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<GLLocation> _glLocationRepository;

        public GeneralLedgerReportAppService(IRepository<ChartofControl, string> chartofControlRepository, IRepository<GLLocation> glLocationRepository, IRepository<GLTRHeader> gltrHeaderRepository, IRepository<GLTRDetail> gltrDetailRepository, IRepository<AccountSubLedger> accountSubLedgerRepository)
        {
            _chartofControlRepository = chartofControlRepository;
            _gltrHeaderRepository = gltrHeaderRepository;
            _gltrDetailRepository = gltrDetailRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _glLocationRepository = glLocationRepository;
        }

        public List<GeneralLedgerDto> GetGeneralLedger(DateTime fromDate, DateTime toDate, string fromAC, string toAC, string status, int fromLocId, int toLocId, int? curRate)
        {
            var tenantId = AbpSession.TenantId;
            DateTime docDate;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<GeneralLedgerDto> generalLedgerDtoList = new List<GeneralLedgerDto>();
            using (SqlConnection cn = new SqlConnection(str))
            {
                string voucherStatus = "";

                using (SqlCommand cmd = new SqlCommand("SP_GeneralLedgerRpt", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromDate", fromDate.Date);
                    cmd.Parameters.AddWithValue("@toDate", toDate.Date);
                    cmd.Parameters.AddWithValue("@tenantId", tenantId);
                    cmd.Parameters.AddWithValue("@fromlocId", fromLocId);
                    cmd.Parameters.AddWithValue("@tolocId", toLocId);
                    cmd.Parameters.AddWithValue("@fromAcc", fromAC);
                    cmd.Parameters.AddWithValue("@toAcc", toAC);
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

                            if (!DBNull.Value.Equals(rdr["DocDate"]))
                            {
                                docDate = (DateTime)rdr["DocDate"];
                            }
                            if (!DBNull.Value.Equals(rdr["DocDate"]))
                            {
                                GeneralLedgerDto generalLedgerDto = new GeneralLedgerDto()
                                {

                                    AccountCode = !DBNull.Value.Equals(rdr["AccountCode"]) ? rdr["AccountCode"].ToString() : "",
                                    AccountTitle = !DBNull.Value.Equals(rdr["AccountTitle"]) ? rdr["AccountTitle"].ToString() : "",
                                    FmtDocNo = !DBNull.Value.Equals(rdr["DocNo"].ToString()) ? (rdr["DocNo"].ToString()) : "",
                                    SubledgerCode = !DBNull.Value.Equals(rdr["SubledgerCode"]) ? Convert.ToInt32(rdr["SubledgerCode"]) : 0,
                                    SubledgerDesc = !DBNull.Value.Equals(rdr["SubledgerDesc"]) ? rdr["SubledgerDesc"].ToString() : "",
                                    Narration = !DBNull.Value.Equals(rdr["Narration"]) ? rdr["Narration"].ToString() : "",
                                    DocDate = Convert.ToDateTime(rdr["DocDate"]),
                                    BookId = !DBNull.Value.Equals(rdr["BookID"]) ? rdr["BookID"].ToString() : "",
                                    ConfigId = !DBNull.Value.Equals(rdr["ConfigID"]) ? Convert.ToInt32(rdr["ConfigID"]) : 0,
                                    Debit = !DBNull.Value.Equals(rdr["LocID"]) ? Convert.ToDouble(rdr["Debit"]) / Convert.ToDouble(curRate) : 0,
                                    Credit = !DBNull.Value.Equals(rdr["LocID"]) ? Convert.ToDouble(rdr["Credit"]) / Convert.ToDouble(curRate) : 0,
                                    Amount = Convert.ToDouble(rdr["Amount"]) / Convert.ToDouble(curRate),
                                    Opening = !DBNull.Value.Equals(rdr["LocID"]) ? Convert.ToDouble(rdr["Opening"]) / Convert.ToDouble(curRate) : 0,
                                    LocId = !DBNull.Value.Equals(rdr["LocID"]) ? Convert.ToInt32(rdr["LocID"]) : 0,
                                    LocDesc = !DBNull.Value.Equals(rdr["LocDesc"]) ? rdr["LocDesc"].ToString() : "",
                                    ChNo = !DBNull.Value.Equals(rdr["ChNumber"]) ? rdr["ChNumber"].ToString() : "",
                                    Status = voucherStatus
                                };
                                generalLedgerDtoList.Add(generalLedgerDto);
                            }

                        }
                    }
                }
                // cn.Close();
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

        public class GeneralLedgerDto
        {
            //public string LocId { get; set; }
            //public string LocDesc { get; set; }
            public string ChNo { get; set; }
            public string Status { get; set; }
            public string AccountCode { get; set; }
            public string ChNumber { get; set; }

            public string AccountTitle { get; set; }

            public int DocNo { get; set; }
            public string FmtDocNo { get; set; }

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
            public string LedgerDesc { get; set; }
        }

    }
}
