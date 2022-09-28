using Abp.Domain.Repositories;
using ERP.CommonServices;
using ERP.GeneralLedger.DirectInvoice;
using ERP.GeneralLedger.SetupForms;
using ERP.Reports.Finance.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.Finance
{
    class SalesTaxWithheldReportAppService : ERPReportAppServiceBase, ISalesTaxWithheldReportAppService
    {
        private readonly IRepository<GLINVHeader> _glinvHeaderRepository;
        private readonly IRepository<GLINVDetail> _glinvDetailRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<TaxClass, int> _taxClassRepository;


        public SalesTaxWithheldReportAppService(IRepository<GLINVHeader> glinvHeaderRepository, IRepository<GLINVDetail> glinvDetailRepository,
            IRepository<AccountSubLedger> accountSubLedgerRepository,
            IRepository<TaxClass, int> taxClassRepository)
        {
            _glinvHeaderRepository = glinvHeaderRepository;
            _glinvDetailRepository = glinvDetailRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _taxClassRepository = taxClassRepository;
        }
        public List<SalesTaxWithheldReportDto> GetData(int? TenantId, string fromDate, string toDate, string fromAcc, string toAcc, string fromSubAcc, string toSubAcc, string taxAuth, string taxClass, string type)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;
            }

            var GLINVHeader = _glinvHeaderRepository.GetAll().Where(o => o.TenantId == TenantId
                             && Convert.ToDateTime(o.DocDate).Date >= Convert.ToDateTime(fromDate).Date
                             && Convert.ToDateTime(o.DocDate).Date <= Convert.ToDateTime(toDate).Date
                             && o.TaxAuth == taxAuth && o.TaxClass == Convert.ToInt32(taxClass) && o.TypeID == type);
            var GLINVDetail = _glinvDetailRepository.GetAll().Where(o => o.TenantId == TenantId && o.AccountID.CompareTo(fromAcc) >= 0 && o.AccountID.CompareTo(toAcc) <= 0 && o.SubAccID>0
            && o.SubAccID >= Convert.ToInt32(fromSubAcc) && o.SubAccID <= Convert.ToInt32(toSubAcc));
            var AccountSubLedger = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == TenantId);


            var data = from H in GLINVHeader
                       join D in GLINVDetail on H.Id equals D.DetID
                       join SL in AccountSubLedger on new { A = D.AccountID, B = (int)D.SubAccID } equals new { A = SL.AccountID, B = SL.Id } into j
                       join T in _taxClassRepository.GetAll().Where(o => o.TenantId == TenantId)
                       on new { A = Convert.ToInt32(H.TaxClass), B = H.TaxAuth } equals new { A = T.CLASSID, B = T.TAXAUTH }
                       orderby H.DocNo
                       from s in j.DefaultIfEmpty()
                       select new SalesTaxWithheldReportDto()
                       {
                           VoucherDate = H.DocDate.ToString("dd-MMM-yyyy"),
                           VoucherNo = H.DocNo.ToString(),
                           CustomerInvoiceNo = H.PartyInvNo,
                           CustomerCode = D.SubAccID.ToString(),
                           CustomerName = s == null ? "" : s.SubAccName,
                           GrossAmt = Math.Abs((double)D.Amount).ToString("N2"),
                           SalesTaxAmt = H.TaxAmount.Value.ToString("N2"),
                           SalesTaxWithheld = H.TaxRate.ToString(),
                       };

            return data.ToList();
        }

    }
}
