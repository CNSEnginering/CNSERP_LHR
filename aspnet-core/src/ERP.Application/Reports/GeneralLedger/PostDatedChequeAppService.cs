using Abp.Domain.Repositories;
using ERP.CommonServices;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.Transaction;
using ERP.Reports.GeneralLedger.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;

namespace ERP.Reports.GeneralLedger
{
    class PostDatedChequeAppService : ERPReportAppServiceBase, IPostDatedChequeAppService
    {
        private readonly IRepository<GlCheque> _glChequeRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedger;
        private readonly IRepository<Bank> _bankRepository;


        public PostDatedChequeAppService(IRepository<GlCheque> glChequeRepository, IRepository<AccountSubLedger> accountSubLedger,
            IRepository<Bank> bankRepository)
        {
            _glChequeRepository = glChequeRepository;
            _accountSubLedger = accountSubLedger;
            _bankRepository = bankRepository;
        }
        public List<PostDatedChequeDto> GetData(int? TenantId, string fromCode, string toCode, string fromDate, string toDate, int? typeID)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;
            }

            int ChequeStatusFilter = 0;
            IQueryable<GlCheque> glCheques;

            glCheques = _glChequeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
               .WhereIf(Convert.ToInt32(ChequeStatusFilter) != 0, e => e.ChequeStatus == Convert.ToInt32(ChequeStatusFilter))
               .WhereIf(typeID != null, e => e.TypeID == typeID)
               .WhereIf(fromCode != null, e => e.DocID >= Convert.ToInt32(fromCode))
               .WhereIf(toCode != null, e => e.DocID <= Convert.ToInt32(toCode))
               .WhereIf(fromDate != null, e => e.EntryDate >= Convert.ToDateTime(fromDate))
               .WhereIf(toDate != null, e => e.EntryDate <= Convert.ToDateTime(toDate).AddDays(1));

            //var glCheques = _glChequeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeID == typeID
            // && o.DocID >= Convert.ToInt32(fromCode) && o.DocID <= Convert.ToInt32(toCode)
            // && o.EntryDate >= Convert.ToDateTime(fromDate) && o.EntryDate <= Convert.ToDateTime(toDate));

            var data = from o in glCheques
                       join
                       a in _accountSubLedger.GetAll()
                       on new { A = Convert.ToInt32(o.PartyID), B = o.TenantId, C = o.AccountID } equals new { A = a.Id, B = a.TenantId, C = a.AccountID }
                       into accJoin
                       from acc in accJoin.DefaultIfEmpty()
                       join
                       c in _bankRepository.GetAll()
                       on new { A = o.BankID, B = o.TenantId } equals new { A = c.BANKID, B = c.TenantId }
                       into bankJoin
                       from bank in bankJoin.DefaultIfEmpty()
                       where (o.TenantId == AbpSession.TenantId)
                       select new PostDatedChequeDto()
                       {
                           DocID = o.DocID,
                           TypeID = o.TypeID == 1 ? "Received" : "Issued",
                           PartyName = acc.SubAccName,
                           EntryDate = o.EntryDate.Value.ToString("dd/MM/yyyy"),
                           BankDesc = bank.BANKNAME,
                           ChequeDate = o.ChequeDate.Value.ToString("dd/MM/yyyy"),
                           ChequeAmt = o.ChequeAmt != null ? o.ChequeAmt.Value.ToString("N2") : "",
                           ChequeNo = o.ChequeNo,
                           StatusDate = o.StatusDate != null ? o.StatusDate.Value.ToString("dd/MM/yyyy") : "",
                           ChequeStatus = (o.ChequeStatus == 1) ? "Collected" : (o.ChequeStatus == 2) ? "Issued"
                           : (o.ChequeStatus == 3) ? "Deposited" : (o.ChequeStatus == 4) ? "Cleared"
                           : (o.ChequeStatus == 5) ? "Cancelled" : (o.ChequeStatus == 6) ? "Holded"
                           : (o.ChequeStatus == 7) ? "Bounced" : ""

                           //PartyBank = o.PartyBank,
                           //ChequeRef = o.ChequeRef,
                           //Remarks = o.Remarks,
                           //LocationID = o.LocationID,
                           //AccountID = o.AccountID,
                           //PartyID = o.PartyID,
                           //BankID = o.BankID,
                       };

            return data.ToList();

        }
    }

}
