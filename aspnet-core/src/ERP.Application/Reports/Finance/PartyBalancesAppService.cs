using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using System.Linq;
using System.Linq.Dynamic.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.Finance
{
    public class PartyBalancesAppService : ERPReportAppServiceBase
    {
        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;
        private readonly IRepository<GLTRDetail> _gltrDetailRepository;
        private readonly IRepository<AccountSubLedger> _subLedgerRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<ControlDetail> _controlDetailRepository;
        private readonly IRepository<GLLocation> _glLocationRepository;
        public PartyBalancesAppService(IRepository<GLTRHeader> gltrHeaderRepository,
            IRepository<GLTRDetail> gltrDetailRepository,
            IRepository<AccountSubLedger> subLedgerRepository,
            IRepository<ChartofControl, string> chartofControlRepository,
            IRepository<ControlDetail> controlDetailRepository,
            IRepository<GLLocation> glLocationRepository
            )
        {
            _gltrHeaderRepository = gltrHeaderRepository;
            _gltrDetailRepository = gltrDetailRepository;
            _subLedgerRepository = subLedgerRepository;
            _controlDetailRepository = controlDetailRepository;
            _chartofControlRepository = chartofControlRepository;
            _glLocationRepository = glLocationRepository;
        }

        public List<PartyBalances> GetData(DateTime fromDate, DateTime toDate, string FromAcc, string ToAcc, int tenantId, string status, int locId)
        {
            List<PartyBalances> partyBalances = new List<PartyBalances>();
            List<GlHeaderList> glHeaderLists = new List<GlHeaderList>();
            IQueryable<GLLocation> locations;
            if (locId == 0)
            {
                locations = _glLocationRepository.GetAll().Where(o => o.TenantId == tenantId);
            }
            else
            {
                locations = _glLocationRepository.GetAll().Where(o => o.TenantId == tenantId && o.LocId == locId);
            }
            if (status == "Approved")
            {
                glHeaderLists = (from a in _gltrHeaderRepository.GetAll()
                                 join
                                 b in _gltrDetailRepository.GetAll()
                                 on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                 join
                                 c in locations
                                 on new { A = a.LocId, B = a.TenantId } equals new { A = c.LocId, B = c.TenantId }
                                 where (a.TenantId == tenantId && b.AccountID.CompareTo(FromAcc) >= 0 && b.AccountID.CompareTo(ToAcc) <= 0 && a.DocDate.Date >= fromDate.Date && a.DocDate <= toDate.Date
                                 && a.Approved == true && a.Posted == false
                                 )
                                 orderby (b.AccountID)
                                 orderby (b.SubAccID)
                                 group new { }
                                 by new
                                 {
                                     b.AccountID,
                                     b.SubAccID,
                                     b.TenantId,
                                     b.LocId,
                                     c.LocDesc
                                 } into g
                                 select new GlHeaderList()
                                 {
                                     AccountID = g.Key.AccountID,
                                     SubAccID = g.Key.SubAccID,
                                     TenantId = g.Key.TenantId,
                                     LocId = g.Key.LocId,
                                     LocDesc = g.Key.LocDesc
                                 }).ToList();
            }
            else if (status == "Posted")
            {
                glHeaderLists = (from a in _gltrHeaderRepository.GetAll()
                                 join
                                 b in _gltrDetailRepository.GetAll()
                                 on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                 join
                                 c in locations
                                 on new { A = a.LocId, B = a.TenantId } equals new { A = c.LocId, B = c.TenantId }
                                 where (a.TenantId == tenantId && b.AccountID.CompareTo(FromAcc) >= 0 && b.AccountID.CompareTo(ToAcc) <= 0 && a.DocDate.Date >= fromDate.Date && a.DocDate <= toDate.Date
                                 && a.Approved == true && a.Posted == true
                                 )
                                 orderby (b.AccountID)
                                 orderby (b.SubAccID)
                                 group new { }
                                 by new
                                 {
                                     b.AccountID,
                                     b.SubAccID,
                                     b.TenantId,
                                     b.LocId,
                                     c.LocDesc
                                 } into g
                                 select new GlHeaderList()
                                 {
                                     AccountID = g.Key.AccountID,
                                     SubAccID = g.Key.SubAccID,
                                     TenantId = g.Key.TenantId,
                                     LocId = g.Key.LocId,
                                     LocDesc = g.Key.LocDesc

                                 }).ToList();
            }
            else
            {
                glHeaderLists = (from a in _gltrHeaderRepository.GetAll()
                                 join
                                 b in _gltrDetailRepository.GetAll()
                                 on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                 join
                                 c in locations
                                 on new { A = a.LocId, B = a.TenantId } equals new { A = c.LocId, B = c.TenantId }
                                 where (a.TenantId == tenantId && b.AccountID.CompareTo(FromAcc) >= 0 && b.AccountID.CompareTo(ToAcc) <= 0 && a.DocDate.Date >= fromDate.Date && a.DocDate <= toDate.Date
                                && a.Approved == true
                                 )
                                 orderby (b.AccountID)
                                 orderby (b.SubAccID)
                                 group new { }
                                 by new
                                 {
                                     b.AccountID,
                                     b.SubAccID,
                                     b.TenantId,
                                     b.LocId,
                                     c.LocDesc
                                 } into g
                                 select new GlHeaderList()
                                 {
                                     AccountID = g.Key.AccountID,
                                     SubAccID = g.Key.SubAccID,
                                     TenantId = g.Key.TenantId,
                                     LocId = g.Key.LocId,
                                     LocDesc = g.Key.LocDesc
                                 }).ToList();
            }
            if (status == "Approved")
            {
                foreach (var data in glHeaderLists)
                {
                    partyBalances.Add(new PartyBalances()
                    {
                        AccountId = data.AccountID,
                        LocId = data.LocId,
                        LocDesc = data.LocDesc,
                        SubAccId = data.SubAccID,
                        SubAccountName = data.SubAccID == 0 ? "" : _subLedgerRepository.GetAll().Where(a => a.TenantId == data.TenantId && a.Id == data.SubAccID && a.AccountID == data.AccountID).SingleOrDefault().SubAccName,
                        AccountName = _chartofControlRepository.GetAll().Where(a => a.TenantId == data.TenantId && a.Id == data.AccountID).SingleOrDefault().AccountName,
                        Opening = (from a in _gltrHeaderRepository.GetAll()
                                   join b in _gltrDetailRepository.GetAll() on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                   where (b.AccountID == data.AccountID && b.SubAccID == data.SubAccID && b.TenantId == data.TenantId && a.DocDate.Date < fromDate.Date && a.Approved == true && a.Posted == false)
                                   select (b.Amount)
                                   ).Sum(),
                        Closing = (from a in _gltrHeaderRepository.GetAll()
                                   join b in _gltrDetailRepository.GetAll() on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                   where (b.AccountID == data.AccountID && b.SubAccID == data.SubAccID && b.TenantId == data.TenantId && a.DocDate.Date <= toDate.Date && a.Approved == true && a.Posted == false)
                                   select (b.Amount)
                                  ).Sum(),
                        Debit = (from a in _gltrHeaderRepository.GetAll()
                                 join b in _gltrDetailRepository.GetAll() on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                 where (b.AccountID == data.AccountID && b.SubAccID == data.SubAccID && b.TenantId == data.TenantId && a.DocDate.Date >= fromDate.Date && a.DocDate <= toDate.Date
                                 && b.Amount > 0 && a.Approved == true && a.Posted == false
                                 )
                                 //select Convert.ToDecimal(b.Amount) > 0 ? Convert.ToDecimal(b.Amount) : 0
                                 select (b.Amount)
                                  ).Sum(),
                        Credit = (from a in _gltrHeaderRepository.GetAll()
                                  join b in _gltrDetailRepository.GetAll() on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                  where (b.AccountID == data.AccountID && b.SubAccID == data.SubAccID && b.TenantId == data.TenantId && a.DocDate.Date >= fromDate.Date && a.DocDate <= toDate.Date
                                  && b.Amount < 0 && a.Approved == true && a.Posted == false
                                  )
                                  // select Convert.ToDecimal(b.Amount) < 0 ? (Convert.ToDecimal(b.Amount)) : 0
                                  select (b.Amount)
                                  ).Sum()
                    });
                }
            }
            else if (status == "Posted")
            {
                foreach (var data in glHeaderLists)
                {
                    partyBalances.Add(new PartyBalances()
                    {
                        AccountId = data.AccountID,
                        SubAccId = data.SubAccID,
                        SubAccountName = data.SubAccID == 0 ? "" : _subLedgerRepository.GetAll().Where(a => a.TenantId == data.TenantId && a.Id == data.SubAccID && a.AccountID == data.AccountID).SingleOrDefault().SubAccName,
                        AccountName = _chartofControlRepository.GetAll().Where(a => a.TenantId == data.TenantId && a.Id == data.AccountID).SingleOrDefault().AccountName,
                        Opening = (from a in _gltrHeaderRepository.GetAll()
                                   join b in _gltrDetailRepository.GetAll() on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                   where (b.AccountID == data.AccountID && b.SubAccID == data.SubAccID && b.TenantId == data.TenantId && a.DocDate.Date < fromDate.Date && a.Approved == true && a.Posted == true)
                                   select (b.Amount)
                                   ).Sum(),
                        Closing = (from a in _gltrHeaderRepository.GetAll()
                                   join b in _gltrDetailRepository.GetAll() on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                   where (b.AccountID == data.AccountID && b.SubAccID == data.SubAccID && b.TenantId == data.TenantId && a.DocDate.Date <= toDate.Date && a.Approved == true && a.Posted == true)
                                   select (b.Amount)
                                  ).Sum(),
                        Debit = (from a in _gltrHeaderRepository.GetAll()
                                 join b in _gltrDetailRepository.GetAll() on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                 where (b.AccountID == data.AccountID && b.SubAccID == data.SubAccID && b.TenantId == data.TenantId && a.DocDate.Date >= fromDate.Date && a.DocDate <= toDate.Date
                                 && b.Amount > 0 && a.Approved == true && a.Posted == true
                                 )
                                 //select Convert.ToDecimal(b.Amount) > 0 ? Convert.ToDecimal(b.Amount) : 0
                                 select (b.Amount)
                                  ).Sum(),
                        Credit = (from a in _gltrHeaderRepository.GetAll()
                                  join b in _gltrDetailRepository.GetAll() on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                  where (b.AccountID == data.AccountID && b.SubAccID == data.SubAccID && b.TenantId == data.TenantId && a.DocDate.Date >= fromDate.Date && a.DocDate <= toDate.Date
                                  && b.Amount < 0 && a.Approved == true && a.Posted == true
                                  )
                                  // select Convert.ToDecimal(b.Amount) < 0 ? (Convert.ToDecimal(b.Amount)) : 0
                                  select (b.Amount)
                                  ).Sum()
                    });
                }
            }
            else
            {
                foreach (var data in glHeaderLists)
                {
                    partyBalances.Add(new PartyBalances()
                    {
                        AccountId = data.AccountID,
                        SubAccId = data.SubAccID,
                        SubAccountName = data.SubAccID == 0 ? "" : _subLedgerRepository.GetAll().Where(a => a.TenantId == data.TenantId && a.Id == data.SubAccID && a.AccountID == data.AccountID).SingleOrDefault().SubAccName,
                        AccountName = _chartofControlRepository.GetAll().Where(a => a.TenantId == data.TenantId && a.Id == data.AccountID).SingleOrDefault().AccountName,
                        Opening = (from a in _gltrHeaderRepository.GetAll()
                                   join b in _gltrDetailRepository.GetAll() on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                   where (b.AccountID == data.AccountID && b.SubAccID == data.SubAccID && b.TenantId == data.TenantId && a.DocDate.Date < fromDate.Date && a.Approved == true)
                                   select (b.Amount)
                                   ).Sum(),
                        Closing = (from a in _gltrHeaderRepository.GetAll()
                                   join b in _gltrDetailRepository.GetAll() on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                   where (b.AccountID == data.AccountID && b.SubAccID == data.SubAccID && b.TenantId == data.TenantId && a.DocDate.Date <= toDate.Date && a.Approved == true)
                                   select (b.Amount)
                                  ).Sum(),
                        Debit = (from a in _gltrHeaderRepository.GetAll()
                                 join b in _gltrDetailRepository.GetAll() on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                 where (b.AccountID == data.AccountID && b.SubAccID == data.SubAccID && b.TenantId == data.TenantId && a.DocDate.Date >= fromDate.Date && a.DocDate <= toDate.Date
                                 && b.Amount > 0 && a.Approved == true
                                 )
                                 //select Convert.ToDecimal(b.Amount) > 0 ? Convert.ToDecimal(b.Amount) : 0
                                 select (b.Amount)
                                  ).Sum(),
                        Credit = (from a in _gltrHeaderRepository.GetAll()
                                  join b in _gltrDetailRepository.GetAll() on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                  where (b.AccountID == data.AccountID && b.SubAccID == data.SubAccID && b.TenantId == data.TenantId && a.DocDate.Date >= fromDate.Date && a.DocDate <= toDate.Date
                                  && b.Amount < 0 && a.Approved == true
                                  )
                                  // select Convert.ToDecimal(b.Amount) < 0 ? (Convert.ToDecimal(b.Amount)) : 0
                                  select (b.Amount)
                                  ).Sum()
                    });
                }
            }
            return partyBalances;
        }
    }
    public class GlHeaderList
    {
        public string AccountID { get; set; }
        public int? SubAccID { get; set; }
        public int TenantId { get; set; }
        public int LocId { get; set; }
        public string LocDesc { get; set; }
    }
    public class PartyBalances
    {
        public string AccountId { get; set; }
        public string SubAccountName { get; set; }
        public string AccountName { get; set; }
        public int? SubAccId { get; set; }
        public double? Opening { get; set; }
        public double? Closing { get; set; }
        public double? Debit { get; set; }
        public double? Credit { get; set; }
        public int LocId { get; set; }
        public string LocDesc { get; set; }
    }
}
