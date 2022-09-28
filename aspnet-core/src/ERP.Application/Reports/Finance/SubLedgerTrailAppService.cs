using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections;
using Abp.Collections.Extensions;
using Abp.Linq.Extensions;

namespace ERP.Reports.Finance
{
    public class SubLedgerTrailAppService : ERPReportAppServiceBase, ISubLedgerTrailAppService
    {
        private readonly IRepository<GLCONFIG> _glConfigRepository;
        private readonly IRepository<GLBOOKS> _glBooksRepository;
        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;
        private readonly IRepository<GLTRDetail> _gltrDetailRepository;
        private readonly IRepository<ChartofControl, string> _ChartofAccountRepository;
        private readonly IRepository<AccountSubLedger> _AccountSubLedgerRepository;
        private readonly IRepository<ControlDetail> _Segment1Repository;
        private readonly IRepository<SubControlDetail> _Segment2Repository;
        private readonly IRepository<Segmentlevel3> _Segment3Repository;

        private readonly IRepository<GroupCategory> _GroupCategoryRepository;
        public SubLedgerTrailAppService(IRepository<GLTRHeader> gLTRHeaderrepository,
            IRepository<GLTRDetail> gLTRdetailRepository,
            IRepository<GLCONFIG> glconfigRepository,
            IRepository<GLBOOKS> glBooksRepository,
            IRepository<ChartofControl, string> chartofAccountRepository,
            IRepository<AccountSubLedger> AccountSubLedgerRepository,
            IRepository<ControlDetail> Segment1Repository,
            IRepository<SubControlDetail> Segment2Repository,
            IRepository<Segmentlevel3> Segment3Repository,
            IRepository<GroupCategory> GroupCategoryRepository)
        {
            _gltrHeaderRepository = gLTRHeaderrepository;
            _gltrDetailRepository = gLTRdetailRepository;
            _glConfigRepository = glconfigRepository;
            _glBooksRepository = glBooksRepository;
            _ChartofAccountRepository = chartofAccountRepository;
            _AccountSubLedgerRepository = AccountSubLedgerRepository;
            _Segment1Repository = Segment1Repository;
            _Segment2Repository = Segment2Repository;
            _Segment3Repository = Segment3Repository;
            _GroupCategoryRepository = GroupCategoryRepository;
        }
        public List<SubLedgerTrial> GetAll(DateTime FromDate, DateTime ToDate, string FromAccountID, string ToAccountID, int? FromSubAccID, int? ToSubAccID, int? SLType, int? TenantID, string Status, int? curRate)
        {
            var x = _AccountSubLedgerRepository.GetAll().Where(O => O.TenantId == TenantID)
                                .WhereIf(SLType != null, e => e.SLType == SLType);
            if (TenantID == null)
            {
                TenantID = AbpSession.TenantId;
            }
            IQueryable<voucher> JournalVoucher;
            var subledgerTrail =
                                from asl in _AccountSubLedgerRepository.GetAll().Where(O => O.TenantId == TenantID)
                                .WhereIf(SLType != null, e => e.SLType == SLType)
                                join ac in _ChartofAccountRepository.GetAll() on new { Id = asl.AccountID, asl.TenantId } equals new { ac.Id, ac.TenantId } into ac1
                                from ac in ac1.DefaultIfEmpty()
                                join seg1 in _Segment1Repository.GetAll() on new { id = ac.ControlDetailId, asl.TenantId } equals new { id = seg1.Seg1ID, seg1.TenantId }
                                join seg2 in _Segment2Repository.GetAll() on new { id = ac.SubControlDetailId, asl.TenantId } equals new { id = seg2.Seg2ID, seg2.TenantId }
                                join seg3 in _Segment3Repository.GetAll() on new { id = ac.Segmentlevel3Id, asl.TenantId } equals new { id = seg3.Seg3ID, seg3.TenantId }
                                join gc in _GroupCategoryRepository.GetAll() on new { id = seg1.Family, seg1.TenantId } equals new { id = gc.GRPCTCODE, gc.TenantId }

                                select new { seg1.Seg1ID, Seg1Name = seg1.SegmentName, seg2.Seg2ID, Seg2Name = seg2.SegmentName, seg3.Seg3ID, Seg3name = seg3.SegmentName, AccountID = ac.Id, ac.AccountName, asl.Id, asl.SubAccName, ac.SubLedger, gc.GRPCTDESC };

            //if (Status == "Approved")
            //{
            JournalVoucher = (from head in _gltrHeaderRepository.GetAll().Where(o => o.TenantId == TenantID)
                              join detail in _gltrDetailRepository.GetAll() on new { id = head.Id, head.TenantId } equals new { id = detail.DetID, detail.TenantId }
                              where DateTime.Parse(head.DocDate.ToLongDateString()) < DateTime.Parse(FromDate.ToLongDateString()) && head.Posted == false && head.Approved == true
                              && String.Compare(detail.AccountID, FromAccountID) >= 0 && String.Compare(detail.AccountID, ToAccountID) <= 0
                              && detail.SubAccID >= FromSubAccID && detail.SubAccID <= ToSubAccID
                              select new voucher { AccountID = detail.AccountID, SubAccID = detail.SubAccID, TenantId = head.TenantId, Opn = detail.Amount, Cur = (double?)0.00 }).Concat(
          from head in _gltrHeaderRepository.GetAll().Where(o => o.TenantId == TenantID)
          join detail in _gltrDetailRepository.GetAll() on new { id = head.Id, head.TenantId } equals new { id = detail.DetID, detail.TenantId }
          where DateTime.Parse(head.DocDate.ToLongDateString()) >= DateTime.Parse(FromDate.ToLongDateString()) && DateTime.Parse(head.DocDate.ToLongDateString()) <= DateTime.Parse(ToDate.ToLongDateString()) 
          //&& head.Posted == false && head.Approved == true
          && String.Compare(detail.AccountID, FromAccountID) >= 0 && String.Compare(detail.AccountID, ToAccountID) <= 0
          && detail.SubAccID >= FromSubAccID && detail.SubAccID <= ToSubAccID
          select new voucher { AccountID = detail.AccountID, SubAccID = detail.SubAccID, TenantId = head.TenantId, Opn = (double?)0.00, Cur = detail.Amount }
          );
            //}
            //else if (Status == "Posted")
            //{
            //    JournalVoucher = (from head in _gltrHeaderRepository.GetAll().Where(o => o.TenantId == TenantID)
            //                      join detail in _gltrDetailRepository.GetAll() on new { id = head.Id, head.TenantId } equals new { id = detail.DetID, detail.TenantId }
            //                      where DateTime.Parse(head.DocDate.ToLongDateString()) < DateTime.Parse(FromDate.ToLongDateString()) && head.Posted == true && head.Approved == true
            //                      && String.Compare(detail.AccountID, FromAccountID) >= 0 && String.Compare(detail.AccountID, ToAccountID) <= 0
            //                      && detail.SubAccID >= FromSubAccID && detail.SubAccID <= ToSubAccID
            //                      select new voucher { AccountID = detail.AccountID, SubAccID = detail.SubAccID, TenantId = head.TenantId, Opn = detail.Amount, Cur = (double?)0.00 }).Concat(
            //  from head in _gltrHeaderRepository.GetAll().Where(o => o.TenantId == TenantID)
            //  join detail in _gltrDetailRepository.GetAll() on new { id = head.Id, head.TenantId } equals new { id = detail.DetID, detail.TenantId }
            //  where DateTime.Parse(head.DocDate.ToLongDateString()) >= DateTime.Parse(FromDate.ToLongDateString()) && DateTime.Parse(head.DocDate.ToLongDateString()) <= DateTime.Parse(ToDate.ToLongDateString()) && head.Posted == true && head.Approved == true
            //  && String.Compare(detail.AccountID, FromAccountID) >= 0 && String.Compare(detail.AccountID, ToAccountID) <= 0
            //  && detail.SubAccID >= FromSubAccID && detail.SubAccID <= ToSubAccID
            //  select new voucher { AccountID = detail.AccountID, SubAccID = detail.SubAccID, TenantId = head.TenantId, Opn = (double?)0.00, Cur = detail.Amount }
            //  );
            //}
            //else
            //{
            //    JournalVoucher = (from head in _gltrHeaderRepository.GetAll().Where(o => o.TenantId == TenantID)
            //                      join detail in _gltrDetailRepository.GetAll() on new { id = head.Id, head.TenantId } equals new { id = detail.DetID, detail.TenantId }
            //                      where DateTime.Parse(head.DocDate.ToLongDateString()) < DateTime.Parse(FromDate.ToLongDateString()) && head.Approved == true
            //                      && String.Compare(detail.AccountID, FromAccountID) >= 0 && String.Compare(detail.AccountID, ToAccountID) <= 0
            //                      && detail.SubAccID >= FromSubAccID && detail.SubAccID <= ToSubAccID
            //                      select new voucher { AccountID = detail.AccountID, SubAccID = detail.SubAccID, TenantId = head.TenantId, Opn = detail.Amount, Cur = (double?)0.00 }).Concat(
            //  from head in _gltrHeaderRepository.GetAll().Where(o => o.TenantId == TenantID)
            //  join detail in _gltrDetailRepository.GetAll() on new { id = head.Id, head.TenantId } equals new { id = detail.DetID, detail.TenantId }
            //  where DateTime.Parse(head.DocDate.ToLongDateString()) >= DateTime.Parse(FromDate.ToLongDateString()) && DateTime.Parse(head.DocDate.ToLongDateString()) <= DateTime.Parse(ToDate.ToLongDateString()) && head.Approved == true
            //  && String.Compare(detail.AccountID, FromAccountID) >= 0 && String.Compare(detail.AccountID, ToAccountID) <= 0
            //  && detail.SubAccID >= FromSubAccID && detail.SubAccID <= ToSubAccID
            //  select new voucher { AccountID = detail.AccountID, SubAccID = detail.SubAccID, TenantId = head.TenantId, Opn = (double?)0.00, Cur = detail.Amount }
            //  );
            //}


            var subledgerTrail1 = from jv in JournalVoucher
                                  join sl in subledgerTrail on new { jv.AccountID, Id = (int)jv.SubAccID } equals new { sl.AccountID, sl.Id }
                                  group new { jv, sl } by new { sl.Seg1ID, sl.Seg1Name, sl.Seg2ID, sl.Seg2Name, sl.Seg3ID, sl.Seg3name, sl.AccountID, sl.AccountName, sl.Id, sl.SubAccName, sl.SubLedger, sl.GRPCTDESC } into g
                                  select new SubLedgerTrial
                                  {
                                      Seg1ID = g.Key.Seg1ID,
                                      Seg1Name = g.Key.Seg1Name,
                                      Seg2ID = g.Key.Seg2ID,
                                      Seg2Name = g.Key.Seg2Name,
                                      Seg3ID = g.Key.Seg3ID,
                                      Seg3Name = g.Key.Seg3name,
                                      AccountID = g.Key.AccountID,
                                      AccountName = g.Key.AccountName,
                                      SubAccID = g.Key.Id,
                                      SubAccName = g.Key.SubAccName,
                                      SubLedger = g.Key.SubLedger,
                                      Family = g.Key.GRPCTDESC,
                                      Opn = g.Select(o => o.jv.Opn) == null ? 0.00 : (double)g.Sum(o => o.jv.Opn)
                                      // / Convert.ToDouble(curRate)
                                      ,
                                      Cur = g.Select(o => o.jv.Cur) == null ? 0.00 : (double)g.Sum(o => o.jv.Cur)
                                      // / Convert.ToDouble(curRate)
                                      ,
                                      ThisMonth = ((g.Select(o => o.jv.Opn) == null ? 0.00 : (double)g.Sum(o => o.jv.Opn)) + (g.Select(o => o.jv.Cur) == null ? 0.00 : (double)g.Sum(o => o.jv.Cur)))
                                      /// Convert.ToDouble(curRate)
                                      ,
                                      Debit = (g.Select(o => o.jv.Cur) == null ? 0.00 : g.Sum(o => o.jv.Cur) > 0 ? (double)g.Sum(o => o.jv.Cur) : 0)
                                      /// Convert.ToDouble(curRate)
                                      ,
                                      Credit = (g.Select(o => o.jv.Cur) == null ? 0.00 : g.Sum(o => o.jv.Cur) < 0 ? (double)g.Sum(o => o.jv.Cur) : 0)
                                      // / Convert.ToDouble(curRate)
                                      ,
                                  };
            return new List<SubLedgerTrial>(
                subledgerTrail1.ToList()
                );

        }
    }

    public class voucher
    {
        public string AccountID { get; set; }
        public int? SubAccID { get; set; }
        public int TenantId { get; set; }
        public double? Opn { get; set; }
        public double? Cur { get; set; }
    }
    public class SubLedgerTrial
    {
        public string Seg1ID { get; set; }
        public string Seg1Name { get; set; }
        public string Seg2ID { get; set; }
        public string Seg2Name { get; set; }
        public string Seg3ID { get; set; }
        public string Seg3Name { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public int SubAccID { get; set; }
        public string SubAccName { get; set; }
        public bool SubLedger { get; set; }
        public string Family { get; set; }
        public double Opn { get; set; }
        public double Cur { get; set; }

        public double ThisMonth { get; set; }

        public double Debit { get; set; }
        public double Credit { get; set; }

    }
}
