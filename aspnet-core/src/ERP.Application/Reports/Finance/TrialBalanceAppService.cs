using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.CommonServices;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ERP.Reports.Finance
{
    public class TrialBalanceAppService : ERPReportAppServiceBase, ITrialBalanceAppService
    {
        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;
        private readonly IRepository<GLTRDetail> _gltrDetailRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<GroupCategory> _groupCategory;
        private readonly IRepository<ControlDetail> _seg1Repository;
        private readonly IRepository<SubControlDetail> _seg2Repository;
        private readonly IRepository<Segmentlevel3> _seg3Repository;
        private readonly IRepository<GLLocation> _glLocationRepository;
        public TrialBalanceAppService(IRepository<GLTRHeader> gLTRHeaderrepository,
            IRepository<GLTRDetail> gLTRdetailRepository,
            IRepository<ChartofControl, string> chartofControlRepository,
            IRepository<GroupCategory> groupCategory,
            IRepository<ControlDetail> seg1Repository,
            IRepository<SubControlDetail> seg2Repository,
            IRepository<Segmentlevel3> seg3Repository,
            IRepository<GLLocation> glLocationRepository
            )
        {
            _chartofControlRepository = chartofControlRepository;
            _gltrHeaderRepository = gLTRHeaderrepository;
            _gltrDetailRepository = gLTRdetailRepository;
            _groupCategory = groupCategory;
            _seg1Repository = seg1Repository;
            _seg2Repository = seg2Repository;
            _seg3Repository = seg3Repository;
            _glLocationRepository = glLocationRepository;
        }
        public List<TrialBalanceDto> GetData(int tenantId, string fromDate, string toDate, string fromAcc, string toAcc, int status, int locId, bool includeZeroBalance, string curRate)
        {
            int TenantId = (int)AbpSession.TenantId;
            SqlCommand cmd;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<TrialBalanceDto> TrailBalanceDtoList = new List<TrialBalanceDto>();
            using (SqlConnection cn = new SqlConnection(str))
            {
                cmd = new SqlCommand("sp_TrialBalance", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", toDate);
                cmd.Parameters.AddWithValue("@FromAccountID", fromAcc);
                cmd.Parameters.AddWithValue("@ToAccountID", toAcc);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@locId", locId);
                cmd.Parameters.AddWithValue("@CurrID", curRate);
                cn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {


                        TrialBalanceDto tbData = new TrialBalanceDto();

                        tbData.AccountId = rdr["AccountID"] is DBNull ? "" : rdr["AccountID"].ToString();
                        tbData.Seg1 = rdr["Segment1"] is DBNull ? "" : rdr["Segment1"].ToString();
                        tbData.Seg2 = rdr["Segment2"] is DBNull ? "" : rdr["Segment2"].ToString();
                        tbData.Seg3 = rdr["Segment3"] is DBNull ? "" : rdr["Segment3"].ToString();
                        tbData.Seg1Name = rdr["Seg1Name"] is DBNull ? "" : rdr["Seg1Name"].ToString();
                        tbData.Seg2Name = rdr["Seg2Name"] is DBNull ? "" : rdr["Seg2Name"].ToString();
                        tbData.Seg3Name = rdr["Seg3Name"] is DBNull ? "" : rdr["Seg3Name"].ToString();
                        tbData.AccountName = rdr["AccountName"] is DBNull ? "" : rdr["AccountName"].ToString();
                        tbData.Family = rdr["Family"] is DBNull ? "" : rdr["Family"].ToString();
                        tbData.Credit = rdr["ThisMonthCr"] is DBNull ? 0.0 : Convert.ToDouble(rdr["ThisMonthCr"].ToString());
                        tbData.Debit = rdr["ThisMonthDr"] is DBNull ? 0.0 : Convert.ToDouble(rdr["ThisMonthDr"].ToString());
                        tbData.OpeningCredit = Convert.ToDouble(rdr["OpeningCredit"].ToString());
                        tbData.OpeningDebit = Convert.ToDouble(rdr["OpeningDebit"].ToString());
                        TrailBalanceDtoList.Add(tbData);

                    }
                }
            }

            if (includeZeroBalance == true)
            {
                var Data = includeZeroBalance == true ? TrailBalanceDtoList.ToList() : TrailBalanceDtoList.Where(o => o.Debit > 0 || o.Credit > 0 || o.OpeningDebit > 0 || o.OpeningCredit < 0
                ).ToList();
                return Data;
            }

            return TrailBalanceDtoList;
        }

        //public List<TrialBalanceDto> GetData(int tenantId, DateTime fromDate, DateTime toDate, string fromAcc, string toAcc, string status, int locId, bool includeZeroBalance, int? curRate)
        //{
        //    tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
        //    IQueryable<GLTRHeader> gLheader;
        //    IQueryable<GLTRHeader> gLheaderOpening;
        //    IQueryable<GLLocation> locations;
        //    locations = _glLocationRepository.GetAll().Where(o => o.TenantId == tenantId);
        //    if (status == "Approved")
        //    {
        //        if (locId == 0)
        //        {
        //            gLheader = _gltrHeaderRepository.GetAll().Where(o => o.TenantId == tenantId && o.DocDate.Date >= fromDate.Date && o.DocDate.Date <= toDate.Date && o.Approved == true && o.Posted == false);
        //            gLheaderOpening = _gltrHeaderRepository.GetAll().Where(o => o.TenantId == tenantId && o.DocDate.Date < fromDate.Date && o.Approved == true && o.Posted == false);
        //        }
        //        else
        //        {
        //            gLheader = _gltrHeaderRepository.GetAll().Where(o => o.TenantId == tenantId && o.DocDate.Date >= fromDate.Date && o.DocDate.Date <= toDate.Date && o.Approved == true && o.Posted == false && o.LocId == locId);
        //            gLheaderOpening = _gltrHeaderRepository.GetAll().Where(o => o.TenantId == tenantId && o.DocDate.Date < fromDate.Date && o.Approved == true && o.Posted == false && o.LocId == locId);
        //        }
        //    }
        //    else if (status == "Posted")
        //    {
        //        if (locId == 0)
        //        {
        //            gLheader = _gltrHeaderRepository.GetAll().Where(o => o.TenantId == tenantId && o.Approved == true && o.Posted == true && o.Approved == true && o.Posted == true);
        //            gLheaderOpening = _gltrHeaderRepository.GetAll().Where(o => o.TenantId == tenantId && o.DocDate.Date < fromDate.Date && o.Approved == true && o.Posted == true);
        //        }
        //        else
        //        {
        //            gLheader = _gltrHeaderRepository.GetAll().Where(o => o.TenantId == tenantId && o.Approved == true && o.Posted == true && o.Approved == true && o.Posted == true && o.LocId == locId);
        //            gLheaderOpening = _gltrHeaderRepository.GetAll().Where(o => o.TenantId == tenantId && o.DocDate.Date < fromDate.Date && o.Approved == true && o.Posted == true && o.LocId == locId);
        //        }
        //    }
        //    else
        //    {
        //        if (locId == 0)
        //        {
        //            gLheader = _gltrHeaderRepository.GetAll().Where(o => o.TenantId == tenantId && o.Approved == true);
        //            gLheaderOpening = _gltrHeaderRepository.GetAll().Where(o => o.TenantId == tenantId && o.DocDate.Date < fromDate.Date && o.Approved == true);
        //        }
        //        else
        //        {
        //            gLheader = _gltrHeaderRepository.GetAll().Where(o => o.TenantId == tenantId && o.Approved == true && o.LocId == locId);
        //            gLheaderOpening = _gltrHeaderRepository.GetAll().Where(o => o.TenantId == tenantId && o.DocDate.Date < fromDate.Date && o.Approved == true && o.LocId == locId);
        //        }
        //    }
        //    var gLDetail = _gltrDetailRepository.GetAll().Where(o => o.TenantId == tenantId);
        //    var chartofControls = _chartofControlRepository.GetAll().Where(o => o.TenantId == tenantId && o.Id.CompareTo(fromAcc) >= 0 && o.Id.CompareTo(toAcc) <= 0);
        //    // var seg1 = _seg1Repository.GetAll().Where(o => o.TenantId == TenantId);
        //    // var seg2 = _seg2Repository.GetAll().Where(o => o.TenantId == TenantId);
        //    /// var seg3 = _seg3Repository.GetAll().Where(o => o.TenantId == TenantId);
        //    //var opening = from a in _gltrHeaderRepository.GetAll().Where(o => o.TenantId == TenantId && o.DocDate.Date < FromDate.Date )
        //    var opening = from a in gLheaderOpening
        //                  join
        //                  b in gLDetail on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
        //                  //join c in chartofControls
        //                  //       on new { A = b.AccountID, B = b.TenantId } equals new { A = c.Id, B = c.TenantId }
        //                  where (a.TenantId == tenantId && a.DocDate.Date < fromDate.Date && a.Approved == true)
        //                  group b by b.AccountID into g
        //                  select new
        //                  {
        //                      AccountID = g.Key,
        //                      OpeningAmountDebit = (g.Sum(a => a.Amount) > 0 ? g.Sum(a => a.Amount) : 0) / curRate,
        //                      OpeningAmountCredit = (g.Sum(a => a.Amount) < 0 ? g.Sum(a => a.Amount) : 0) / curRate,
        //                  };

        //    var betweenDatesData = from a in gLheader
        //                           join b in gLDetail
        //                           on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
        //                           //join c in chartofControls
        //                           // on new { A = b.AccountID, B = b.TenantId } equals new { A = c.Id, B = c.TenantId }
        //                           where (a.Approved == true && a.DocDate.Date >= fromDate.Date && a.DocDate <= toDate.Date)
        //                           select new
        //                           {
        //                               AccountId = b.AccountID,
        //                               // AccName = c.AccountName,
        //                               Debit = (b.Amount > 0 ? Convert.ToDouble(b.Amount) : 0) / curRate,
        //                               Credit = (b.Amount < 0 ? Convert.ToDouble(b.Amount) * -1 : 0) / curRate,
        //                               a.TenantId
        //                           };

        //    var trialBalanceDto = from a in chartofControls
        //                          join d in _seg1Repository.GetAll().Where(o => o.TenantId == tenantId)
        //                          on new { A = a.ControlDetailId, B = a.TenantId } equals new { A = d.Seg1ID, B = d.TenantId }
        //                          join e in _seg2Repository.GetAll().Where(o => o.TenantId == tenantId)
        //                          on new { A = a.SubControlDetailId, B = a.TenantId } equals new { A = e.Seg2ID, B = e.TenantId }
        //                          join f in _seg3Repository.GetAll().Where(o => o.TenantId == tenantId)
        //                          on new { A = a.Segmentlevel3Id, B = a.TenantId } equals new { A = f.Seg3ID, B = f.TenantId }
        //                          join g in _groupCategory.GetAll().Where(o => o.TenantId == tenantId)
        //                           on new { A = d.Family, B = d.TenantId } equals new { A = g.GRPCTCODE, B = g.TenantId }

        //                          select new TrialBalanceDto()
        //                          {
        //                              LocDesc = locId == 0 ? "" : locations.Where(o => o.LocId == locId).SingleOrDefault().LocDesc,
        //                              LocId = locId,
        //                              AccountId = a.Id,
        //                              AccountName = a.AccountName,
        //                              Family = g.GRPCTDESC,
        //                              Debit = (betweenDatesData.Where(o => o.TenantId == tenantId && o.AccountId == a.Id).Count() > 0 ?
        //                              betweenDatesData.Where(o => o.TenantId == tenantId && o.AccountId == a.Id).Select(a => a.Debit).Sum() : 0),
        //                              Credit = (betweenDatesData.Where(o => o.TenantId == tenantId && o.AccountId == a.Id).Count() > 0 ?
        //                              betweenDatesData.Where(o => o.TenantId == tenantId && o.AccountId == a.Id).Select(a => a.Credit).Sum() : 0),
        //                              Seg1 = d.Seg1ID,
        //                              Seg2 = e.Seg2ID,
        //                              Seg3 = f.Seg3ID,
        //                              Seg1Name = d.SegmentName,
        //                              Seg2Name = e.SegmentName,
        //                              Seg3Name = f.SegmentName,
        //                              Sl = a.SubLedger,
        //                              OpeningDebit = (opening.Where(o => o.AccountID == a.Id).Select(x => x.OpeningAmountDebit).SingleOrDefault() == null ? 0 :
        //                                opening.Where(o => o.AccountID == a.Id).Select(x => x.OpeningAmountDebit).SingleOrDefault()),
        //                              OpeningCredit = (opening.Where(o => o.AccountID == a.Id).Select(x => x.OpeningAmountCredit).SingleOrDefault() == null ? 0 :
        //                                opening.Where(o => o.AccountID == a.Id).Select(x => x.OpeningAmountCredit).SingleOrDefault())
        //                          };

        //    return includeZeroBalance == true ? trialBalanceDto.ToList() : trialBalanceDto.Where(o => o.Debit > 0 || o.Credit > 0 || o.OpeningDebit > 0 || o.OpeningCredit < 0
        //    ).ToList();
        //}
    }

}
