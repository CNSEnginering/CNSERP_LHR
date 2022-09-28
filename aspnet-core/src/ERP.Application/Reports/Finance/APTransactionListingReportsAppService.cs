using Abp.Domain.Repositories;
using Abp.Web.Models;
using ERP.CommonServices;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using ERP.Authorization.Users;
using ERP.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.AccountPayables.Dtos;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using ERP.GeneralLedger.SetupForms.LedgerType;

namespace ERP.Reports.Finance
{
    public class APTransactionListingReportsAppService : ERPReportAppServiceBase, IAPTransactionListingReportsAppService
    {
        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;
        private readonly IRepository<GLTRDetail> _gltrDetailRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<ControlDetail> _controlDetailRepository;
        private readonly IRepository<GLCONFIG> _glConfigRepository;
        private readonly IRepository<GLBOOKS> _glBooksRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<AccountSubLedger> _accSubLedgerRepository;
        private readonly IRepository<GLLocation> _glLocationRepository;
        private readonly IRepository<LedgerType> _ledgerTypeRepository;
        public APTransactionListingReportsAppService(
            IRepository<GLTRHeader> gLTRHeaderrepository,
            IRepository<GLTRDetail> gLTRdetailRepository,
            IRepository<ChartofControl, string> chartofControlRepository,
            IRepository<ControlDetail> controlDetailRepository,
            IRepository<GLCONFIG> glconfigRepository,
            IRepository<GLBOOKS> glBooksRepository,
            IRepository<User, long> userRepository,
            IRepository<AccountSubLedger> accSubLedgerRepository,
            IRepository<GLLocation> glLocationRepository,
            IRepository<LedgerType> ledgerTypeRepository
            )
        {
            _gltrHeaderRepository = gLTRHeaderrepository;
            _gltrDetailRepository = gLTRdetailRepository;
            _chartofControlRepository = chartofControlRepository;
            _controlDetailRepository = controlDetailRepository;
            _glConfigRepository = glconfigRepository;
            _glBooksRepository = glBooksRepository;
            _userRepository = userRepository;
            _accSubLedgerRepository = accSubLedgerRepository;
            _glLocationRepository = glLocationRepository;
            _ledgerTypeRepository = ledgerTypeRepository;
        }

        public List<TransactionListDto> GetData(DateTime FromDate, DateTime ToDate, string Book, string User, int tenantId, string status, int locId)
        {
            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            IQueryable<GLTRHeader> header;
            List<string> books;
            books = Book != "All" ? _glBooksRepository.GetAll().Where(o => o.TenantId == tenantId && o.BookID == Book).Select(i => i.BookID).ToList() :
                _glBooksRepository.GetAll().Where(o => o.TenantId == tenantId).Select(i => i.BookID).ToList();

            if (Book == "All")
                books.Add("JV");
            else if (Book == "JV")
                books.Add("JV");

            books.ToArray();

            var chartOfAccounts = _chartofControlRepository.GetAll().Where(o => o.TenantId == tenantId);
            var detail = _gltrDetailRepository.GetAll()
                                               .Where(o => o.TenantId == tenantId).
                                               Select(x => new { x.AccountID, x.Narration, x.Amount, x.DetID, x.TenantId, x.SubAccID });
            if (status == "Approved")
            {
                header = _gltrHeaderRepository.GetAll().Where(o => o.TenantId == tenantId && o.Approved == true && o.Posted == false);
            }
            else if (status == "Posted")
            {
                header = _gltrHeaderRepository.GetAll().Where(o => o.TenantId == tenantId && o.Approved == true && o.Posted == true);
            }
            else if (status == "All")
            {
                header = _gltrHeaderRepository.GetAll().Where(o => o.TenantId == tenantId);
            }
            else
            {
                header = _gltrHeaderRepository.GetAll().Where(o => o.TenantId == tenantId && o.Approved == false && o.Posted == false);
            }
            IQueryable<GLLocation> locations;
            if (locId == 0)
            {
                locations = _glLocationRepository.GetAll().Where(o => o.TenantId == tenantId);
            }
            else
            {
                locations = _glLocationRepository.GetAll().Where(o => o.TenantId == tenantId && o.LocId == locId);
            }
            string[] user = User == "All" ? _userRepository.GetAll().Select(o => o.UserName).ToArray() : _userRepository.GetAll().Where(o => o.UserName == User).Select(o => o.UserName).ToArray();
            var transactionListDto = from a in header
                                     join
                                     b in detail
                                     on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                     join
                                     c in chartOfAccounts
                                     .Select(x => new { x.Id, x.AccountName, x.TenantId }) on new { A = b.AccountID, B = b.TenantId } equals new { A = c.Id, B = c.TenantId }
                                     join
                                     d in _accSubLedgerRepository.GetAll().Where(o => o.TenantId == tenantId).Select(x => new { x.Id, x.TenantId, x.AccountID, x.SubAccName, x.SLType })
                                     on new { A = Convert.ToInt32(b.SubAccID), B = b.TenantId, C = b.AccountID } equals new { A = d.Id, B = d.TenantId, C = d.AccountID }
                                     into subAcc
                                     from acc in subAcc.DefaultIfEmpty()
                                     join
                                     e in locations
                                     on new { A = a.LocId, B = a.TenantId } equals new { A = e.LocId, B = e.TenantId }
                                     //join
                                     //j in _ledgerTypeRepository.GetAll().Where(a => a.TenantId == AbpSession.TenantId)
                                     //on new { A = Convert.ToInt32(acc.SLType), B = a.TenantId } equals new { A = j.LedgerID, B = j.TenantId }
                                     where ((a.DocDate.Date >= FromDate.Date && a.DocDate.Date <= ToDate.Date) && user.Contains(a.AuditUser)
                                     && books.Contains(a.BookID)
                                     )
                                     orderby a.FmtDocNo
                                     group new
                                     {
                                         a.BookID,
                                         c.AccountName,
                                         b.Narration,
                                         b.Amount,
                                         a.Posted,
                                         AccountId = b.AccountID,
                                         a.AuditTime,
                                         a.AuditUser,
                                         b.DetID,
                                         a.ConfigID,
                                         a.FmtDocNo,
                                         a.DocDate,
                                         acc.SubAccName,
                                         b.SubAccID,
                                         e.LocId,
                                         e.LocDesc,
                                         a.ChNumber,
                                         acc.SLType,
                                         //  j.LedgerDesc,
                                         a.Approved
                                     } by new
                                     {
                                         a.BookID,
                                         c.AccountName,
                                         b.Narration,
                                         b.Amount,
                                         a.Posted,
                                         AccountId = b.AccountID,
                                         a.AuditTime,
                                         a.AuditUser,
                                         b.DetID,
                                         a.ConfigID,
                                         a.FmtDocNo,
                                         a.DocDate,
                                         acc.SubAccName,
                                         b.SubAccID,
                                         e.LocId,
                                         e.LocDesc,
                                         a.ChNumber,
                                         acc.SLType,
                                         // j.LedgerDesc,
                                         a.Approved
                                     }
                                      into g
                                     select new TransactionListDto()
                                     {
                                         BookId = g.Key.BookID,
                                         ChequeNo = g.Key.ChNumber,
                                         DetId = g.Key.DetID,
                                         AccountName = g.Key.AccountName,
                                         Debit = g.Sum(a => a.Amount) > 0 ? Convert.ToDouble(g.Sum(a => a.Amount)) : 0,
                                         Credit = g.Sum(a => a.Amount) < 0 ? Convert.ToDouble(g.Sum(a => a.Amount) * -1) : 0,
                                         Narration = g.Key.Narration,
                                         Posted = g.Key.Posted,
                                         AccountId = g.Key.AccountId,
                                         ConfigID = g.Key.ConfigID,
                                         BookConfigMerge = g.Key.BookID + "-" + g.Key.ConfigID,
                                         Doc = g.Key.FmtDocNo,
                                         Date = g.Key.DocDate.Date.Day + "-" + g.Key.DocDate.Date.ToString("MMMM") + "-" + g.Key.DocDate.Date.Year,
                                         AudtTime = g.Key.AuditTime,
                                         User = g.Key.AuditUser,
                                         SubAccName = g.Key.SubAccName == null ? "" : g.Key.SubAccName,
                                         SubAccId = g.Key.SubAccID == null ? 0 : g.Key.SubAccID,
                                         LocId = g.Key.LocId,
                                         LocDesc = g.Key.LocDesc,
                                         LedgerDesc = _ledgerTypeRepository.GetAll().Where(a => a.TenantId == AbpSession.TenantId &&
                                         a.LedgerID == g.Key.SLType
                                         ).Count() > 0  ?
                                         _ledgerTypeRepository.GetAll().Where(a => a.TenantId == AbpSession.TenantId &&
                                         a.LedgerID == g.Key.SLType
                                         ).First().LedgerDesc : ""
                                         ,
                                         Status = (g.Key.Approved == true && g.Key.Posted == true ? "Both" : (g.Key.Posted == true ? "Posted" : "Approved"))
                                     };



            return transactionListDto.ToList();
        }

    }

    //public class UserDTO
    //{
    //    public int ID { get; set; }

    //    public string Name { get; set; }
    //}
}
