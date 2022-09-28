using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using ERP.AccountPayables.Dtos;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using ERP.Reports.Dto;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ERP.Reports.Filters
{
    public class VoucherPrintingReportsAppService : ERPReportAppServiceBase
    {

        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;
        private readonly IRepository<GLTRDetail> _gltrDetailRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<ControlDetail> _controlDetailRepository;
        private readonly IRepository<GLCONFIG> _glConfigRepository;
        private readonly IRepository<GLBOOKS> _glBooksRepository;
        private readonly IRepository<ControlDetail> _controlDetail;
        private readonly IRepository<SubControlDetail> _SubControlDetail;
        private readonly IRepository<Segmentlevel3> _Segmentlevel3;
        private readonly IRepository<AccountSubLedger> _SubLedgerRepository;

        public VoucherPrintingReportsAppService(
            IRepository<GLTRHeader> gLTRHeaderrepository,
            IRepository<GLTRDetail> gLTRdetailRepository,
            IRepository<ChartofControl, string> chartofControlRepository,
            IRepository<ControlDetail> controlDetailRepository,
            IRepository<GLCONFIG> glconfigRepository,
            IRepository<GLBOOKS> glBooksRepository,
            IRepository<ControlDetail> controlDetail,
            IRepository<SubControlDetail> SubControlDetail,
            IRepository<Segmentlevel3> Segmentlevel3,
            IRepository<AccountSubLedger> subLedger

            )
        {
            _gltrHeaderRepository = gLTRHeaderrepository;
            _gltrDetailRepository = gLTRdetailRepository;
            _chartofControlRepository = chartofControlRepository;
            _controlDetailRepository = controlDetailRepository;
            _glConfigRepository = glconfigRepository;
            _glBooksRepository = glBooksRepository;
            _controlDetail = controlDetail;
            _SubControlDetail = SubControlDetail;
            _Segmentlevel3 = Segmentlevel3;
            _SubLedgerRepository = subLedger;
        }



        public async Task<ListResultDto<GetBookViewModeldto>> GetBookListForReport()
        {
            var query = _glBooksRepository.GetAll().Select(i => new { i.BookName, i.BookID }).Distinct();
            var groupCategoryList = await query
                .ToListAsync();

            var lookupTableDtoList = new List<GetBookViewModeldto>();
            foreach (var groupCategory in groupCategoryList)
            {
                lookupTableDtoList.Add(new GetBookViewModeldto
                {

                    BookId = groupCategory.BookID,
                    BookName = groupCategory.BookName
                });
            }

            return new ListResultDto<GetBookViewModeldto>(
                lookupTableDtoList
            );
        }
       
        public List<VoucherPrint> GetVoucherPrintingList(string bookId, int year, int month, int fromconfig, int toconfig, int fromdoc, int todoc)
        {
            int tenantId = 10;
            toconfig = 15;
            var books = bookId != "All" ? _glBooksRepository.GetAll().Where(o => o.TenantId == tenantId && o.BookID == bookId).Select(i => new { i.BookName, i.BookID, i.TenantId }) :
                _glBooksRepository.GetAll().Where(o => o.TenantId == tenantId).Select(i => new { i.BookName, i.BookID, i.TenantId });
          
            var chartOfAccounts = _chartofControlRepository.GetAll().Where(o => o.TenantId == tenantId);
            var detail = _gltrDetailRepository.GetAll()
                                               .Where(o => o.TenantId == tenantId).
                                               Select(x => new { x.AccountID, x.Narration, x.Amount, x.DetID, x.TenantId });
            var header = _gltrHeaderRepository.GetAll().Where(o => o.TenantId == tenantId);

            var transactionListDto = from a in header
                                     join
                                     b in detail
                                     on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                     join
                                     c in chartOfAccounts
                                     .Select(x => new { x.Id, x.AccountName, x.TenantId }) on new { A = b.AccountID, B = b.TenantId } equals new { A = c.Id, B = c.TenantId }
                                     join
                                     d in books
                                     on new { A = a.BookID, B = a.TenantId } equals new { A = d.BookID, B = d.TenantId }
                                     where ((a.DocMonth == month) && (a.DocDate.Year == year) && a.TenantId == tenantId && (a.DocNo >= fromdoc && a.DocNo <= todoc) && (a.ConfigID >= fromconfig && a.ConfigID <= toconfig))
                                     orderby a.DocNo
                                     group new
                                     {
                                         d.BookID,
                                         c.AccountName,
                                         b.Narration,
                                         b.Amount,
                                         a.Posted,
                                         AccountId = b.AccountID,
                                      
                                         b.DetID,
                                         a.ConfigID,
                                         a.DocNo,
                                         a.DocDate
                                     } by new
                                     {
                                         d.BookName,
                                         c.AccountName,
                                         b.Narration,
                                         b.Amount,
                                         a.Posted,
                                         AccountId = b.AccountID,
                                         a.DocNo,
                                         a.DocDate
                                     }
                                      into g
                                     select new VoucherPrint()
                                     {
                                         BookId = g.Key.BookName,
                                         AccountName = g.Key.AccountName,
                                         Debit = g.Sum(a => a.Amount) > 0 ? Convert.ToDouble(g.Sum(a => a.Amount)) : 0,
                                         Credit = g.Sum(a => a.Amount) < 0 ? Convert.ToDouble(g.Sum(a => a.Amount) * -1) : 0,
                                         Narration = g.Key.Narration,
                                         Posted = g.Key.Posted == true ? "Posted" : "Un Posted",
                                         AccountId = g.Key.AccountId,
                                         DocNo = g.Key.DocNo,
                                         DocDate = g.Key.DocDate.Date.Day + "-" + g.Key.DocDate.Date.ToString("MMMM") + "-" + g.Key.DocDate.Date.Year,
                                        
                                     };
         //   string ddd = JsonConvert.SerializeObject(transactionListDto);
            return transactionListDto.ToList();
        }
     
        public List<ChartofAccountdto> GetChartofAccountList()
        {

            int tenentid = 1;
            var filteredChartofControls = (from o in _chartofControlRepository.GetAll().Where(x => x.TenantId == tenentid)

                                           join cd in _Segmentlevel3.GetAll().Where(x => x.TenantId == tenentid) on o.Segmentlevel3Id equals cd.Seg3ID into cd1
                                           from s1 in cd1.DefaultIfEmpty()
                                           join scd in _SubControlDetail.GetAll().Where(x => x.TenantId == tenentid) on o.SubControlDetailId equals scd.Seg2ID into scd1
                                           from s2 in scd1.DefaultIfEmpty()
                                           join sg3 in _controlDetail.GetAll().Where(x => x.TenantId == tenentid) on o.ControlDetailId equals sg3.Seg1ID into sg3_1
                                           from s3 in sg3_1.DefaultIfEmpty()
                                           select new ChartofAccountdto()
                                           {
                                               AccountId = o.Id,
                                               AccountName = o.AccountName,
                                               Seg1 = o.ControlDetailId,
                                               Seg2 = o.SubControlDetailId,
                                               Seg3 = o.Segmentlevel3Id,
                                               SubLedger = o.SubLedger == true?"Un Posted":"Posted",
                                               SegName = s3.SegmentName,
                                               Seg2Name = s2.SegmentName,
                                               Seg3Name = s1.SegmentName,
                                               Family = s3.Family == 1 ? "Asset" : "LIABILITY"
                                           }); 

            return filteredChartofControls.ToList();
        }
        public class ChartofAccountdto
        {

            public string AccountId { get; set; }
            public string AccountName { get; set; }
            public string Seg1 { get; set; }
            public string SegName { get; set; }
            public string Seg2 { get; set; }
            public string Seg2Name { get; set; }
            public string Seg3 { get; set; }
            public string Seg3Name { get; set; }
            public string Family { get; set; }
            public string SubLedger { get; set; }
        }
      
        //account aging report
        //      @AsOfDate char (10) = '07/19/2004',	
        //@FromAcc char (11) = '01-001-0001',
        //@ToAcc char (11) = '99-999-9999',
        //@FromSubAcc int = 0,
        //@ToSubAcc int = 0,
        //@Descp varchar(50) = '',
        //@A1 int = 30,
        //@A2 int = 60,
        //@A3 int = 90,
        //@A4 int = 120,
        //@Type smallint = 0

        public class AccountsAging
        {
            public string AccountId { get; set; }
            public string AccountName { get; set; }
            public int SubId { get; set; }
            public string SubAccountTitle { get; set; }
            public double Balance { get; set; }
            public double A1 { get; set; }
            public double A2 { get; set; }
            public double A3 { get; set; }
            public double A4 { get; set; }
            public double A5 { get; set; }
        }
      
        public List<AccountsAging> GetAccountAginReport(DateTime AsOfDate, string FromAcc, string ToAcc, int FromSubAcc, int ToSubAcc, int Ag1, int Ag2, int Ag3, int Ag4, int Type)
        {
            //int? tenantId = AbpSession.TenantId;
            //
            int? tenantId = 10;
            var names = new int[] { FromSubAcc, ToSubAcc };
            DateTime ag1 = AsOfDate.AddDays(-Ag1 + 1);
            var Nag1 = ag1.ToString("MM/dd/yyyy");

            DateTime ag2 = AsOfDate.AddDays(-Ag2 + 1);
            var Nag2 = ag2.ToString("MM/dd/yyyy");

            DateTime ag3 = AsOfDate.AddDays(-Ag3 + 1);
            var Nag3 = ag3.ToString("MM/dd/yyyy");

            DateTime ag4 = AsOfDate.AddDays(-Ag4 + 1);
            var Nag4 = ag4.ToString("MM/dd/yyyy");

            var gLheader = _gltrHeaderRepository.GetAll().Where(o => o.TenantId == tenantId);
            var gLDetail = _gltrDetailRepository.GetAll().Where(o => o.TenantId == tenantId);
            var chartofControls = _chartofControlRepository.GetAll().Where(o => o.TenantId == tenantId && o.Id.CompareTo(FromAcc) >= 0 && o.Id.CompareTo(ToAcc) <= 0);
            var subledger = _SubLedgerRepository.GetAll().Where(a=> a.Id >= FromSubAcc && a.Id <= ToSubAcc && a.TenantId == tenantId);
      
            //var acAging = from a in gLheader
            //              join b in gLDetail
            //              on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
            //              join c in chartofControls
            //              on new { A = b.AccountID, B = b.TenantId } equals new { A = c.Id, B = c.TenantId }
            //              //join sl in subledger
            //              //on new { A = (Int32)b.SubAccID, B = b.TenantId } equals new { A = sl.Id, B = sl.TenantId }
            //              join sll in subledger on new { X = b.AccountID, Y = (int)b.SubAccID } equals new { X = sll.AccountID, Y = sll.Id } into sl
            //              from s in sl.DefaultIfEmpty()
            //              select new
            //              {
            //                  c.Id,
            //                  c.AccountName,
            //                  subid = s.Id,
            //                 sname= s.SubAccName!=null?s.SubAccName:"n/a",
            //                  b.Amount,
            //                  a.DocDate

            //              };
            var acAging = from c in chartofControls
                          join b in gLDetail on c.Id equals b.AccountID
                          join a in gLheader on b.DetID equals a.Id
                          
                          join sl in subledger on new { X = b.AccountID, Y = (int)b.SubAccID,z=b.TenantId } equals new { X = sl.AccountID, Y = sl.Id,z=sl.TenantId } into sb
                          from s in sb.DefaultIfEmpty()
                          select new
                          {
                              c.Id,
                              c.AccountName,
                              subid = (int?)s.Id,
                              sname = s.SubAccName != null ? s.SubAccName : "n/a",
                              b.Amount,
                              a.DocDate

                          };
            var report = (from b in acAging
                          where (b.DocDate <= Convert.ToDateTime(AsOfDate.ToString("MM/dd/yyyy")) && b.Amount > 0)
                          select new { AccountId = b.Id, 
                              SubacId = b.subid, b.AccountName, SubAcName = b.sname != null ? b.sname : "n/a", Balance = b.Amount, A1 = (double?)0.00, A2 = (double?)0, A3 = (double?)0, A4 = (double?)0, A5 = (double?)0 }).Concat
                         (from b in acAging
                          where (b.DocDate <= Convert.ToDateTime(Nag1) && b.Amount > 0)
                           select new { AccountId = b.Id, 
                               SubacId = b.subid, b.AccountName, SubAcName = b.sname != null?b.sname : "N/A", Balance = (double?)0, A1 = b.Amount, A2 = (double?)0, A3 = (double?)0, A4 = (double?)0, A5 = (double?)0 }).Concat
                         (from b in acAging
                          where (b.DocDate <= Convert.ToDateTime(AsOfDate.AddDays(Ag2)) && b.Amount > 0)
                          select new { AccountId = b.Id, 
                              SubacId = b.subid, b.AccountName, SubAcName = b.sname != null?b.sname : "N/A", Balance = (double?)0, A1 = (double?)0, A2 = b.Amount, A3 = (double?)0, A4 = (double?)0, A5 = (double?)0 }).Concat
                         (from b in acAging
                          where (b.DocDate <= Convert.ToDateTime(AsOfDate.AddDays(Ag3)) && b.Amount > 0)
                          select new { AccountId = b.Id, 
                              SubacId = b.subid, b.AccountName, SubAcName = b.sname != null?b.sname : "N/A", Balance = (double?)0, A1 = (double?)0, A2 = (double?)0, A3 = b.Amount, A4 = (double?)0, A5 = (double?)0 }).Concat
                         (from b in acAging
                          where (b.DocDate <= Convert.ToDateTime(AsOfDate.AddDays(Ag4)) && new[] { FromAcc, ToAcc }.Contains(b.Id) && b.Amount > 0)
                          select new { AccountId = b.Id,
                              SubacId = b.subid, b.AccountName, SubAcName = b.sname != null?b.sname : "N/A", Balance = (double?)0, A1 = (double?)0, A2 = (double?)0, A3 = (double?)0, A4 = b.Amount, A5 = (double?)0 }).ToList();
            var getlist = (from o in report
                           where o.Balance > 0
                           orderby o.AccountId descending
                           group o by new
                           {
                               o.AccountId,
                               o.AccountName,
                               o.Balance,
                               o.SubacId,
                               o.SubAcName,
                               o.A5,
                               o.A4,
                               o.A3,
                               o.A2,
                               o.A1
                           } into g
                           select new AccountsAging()
                           {
                               AccountId = g.Key.AccountId,
                               AccountName = g.Key.AccountName,
                               SubId =  Convert.ToInt32(g.Key.SubacId),
                               SubAccountTitle = g.Key.SubAcName,
                               Balance = (double)g.Key.Balance,
                               A1 = (double)g.Key.A1,
                               A2 = (double)g.Key.A2,
                               A3 = (double)g.Key.A3,
                               A4 = (double)g.Key.A4,
                               A5 = (double)g.Key.A5
                           });
            return getlist.ToList();
        }
    }
}

