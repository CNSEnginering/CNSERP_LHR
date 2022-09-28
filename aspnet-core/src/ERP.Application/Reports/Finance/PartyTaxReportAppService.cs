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
using ERP.Reports.Finance.Dto;
using ERP.GeneralLedger.DirectInvoice;

namespace ERP.Reports.Finance
{
    public class PartyTaxReportAppService : ERPReportAppServiceBase , IPartyTaxReportAppService
    {
        private readonly IRepository<CPR> _PartyTaxHeaderRepository;
        private readonly IRepository<GLINVHeader> _glinvHeaderRepository;
        private readonly IRepository<GLINVDetail> _glinvDetailRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepositoryy;
        private readonly IRepository<TaxClass, int> _taxClassRepository;
        public PartyTaxReportAppService(
            IRepository<CPR> PartyTaxHeaderRepository,
            IRepository<GLINVHeader> glinvHeaderRepository,
            IRepository<GLINVDetail> glinvDetailRepository,
            IRepository<AccountSubLedger> accountSubLedgerRepository,
            IRepository<TaxClass, int> taxClassRepository)
        {
            _PartyTaxHeaderRepository = PartyTaxHeaderRepository;
            _glinvHeaderRepository = glinvHeaderRepository;
            _glinvDetailRepository = glinvDetailRepository;
            _accountSubLedgerRepositoryy = accountSubLedgerRepository;
            _taxClassRepository = taxClassRepository;
        }

        public List<PartyTaxReportDto> GetData(int? TenantId, string fromDate, string toDate, string fromAcc, string toAcc, string fromSubAcc, string toSubAcc, string taxAuth, string fromTaxClass, string toTaxClass)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }

            var directInvoiceH = _glinvHeaderRepository.GetAll().Where(o => o.TenantId == TenantId && o.DocDate >= Convert.ToDateTime(fromDate) && o.DocDate <= Convert.ToDateTime(toDate) 
            && taxAuth == o.TaxAuth && o.TaxClass>= Convert.ToInt32(fromTaxClass) && o.TaxClass<= Convert.ToInt32(toTaxClass));
            var directInvoiceD = _glinvDetailRepository.GetAll().Where(o => o.TenantId == TenantId && o.AccountID.CompareTo(fromAcc)>=0 && o.AccountID.CompareTo(toAcc)<=0 
            && o.SubAccID >=Convert.ToInt32(fromSubAcc) && o.SubAccID<=Convert.ToInt32(toSubAcc));

            var data = from a in directInvoiceH
                       join b in directInvoiceD on a.Id equals b.DetID
                       join c in _accountSubLedgerRepositoryy.GetAll().Where(o => o.TenantId == TenantId)
                       on new { A = b.TenantId, B = b.AccountID, C = Convert.ToInt32(b.SubAccID) } equals new { A = c.TenantId, B = c.AccountID, C = c.Id }
                       join d in _taxClassRepository.GetAll().Where(o => o.TenantId == TenantId)
                       on new { A = Convert.ToInt32(a.TaxClass), B = a.TaxAuth } equals new { A = d.CLASSID, B = d.TAXAUTH }
                       orderby a.DocNo
                       select new PartyTaxReportDto
                       {
                           VoucherDate = a.DocDate == null ? a.DocDate.ToString() : a.DocDate.ToString("dd/MM/yyyy"),
                           VoucherNo = a.DocNo.ToString(),
                           CustomerInvoiceNo = a.PartyInvNo,
                           CustomerCode = b.SubAccID.ToString(),
                           CustomerName = c.SubAccName,
                           GrossAmount = b.Amount.ToString(),
                           TaxAmount = a.TaxAmount.ToString(),
                           TaxRate = a.TaxRate.ToString(),
                           TaxClass=d.CLASSDESC
                       };
            return data.ToList();

        }
    }
}
