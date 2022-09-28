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
    public class SalesTaxDeductionAppService : ERPReportAppServiceBase
        //, ISalesTaxDeductionReportAppService
    {
        private readonly IRepository<GLINVHeader> _glinvHeaderRepository;
        private readonly IRepository<GLINVDetail> _glinvDetailRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<TaxClass, int> _taxClassRepository;
        public SalesTaxDeductionAppService(
            IRepository<GLINVHeader> glinvHeaderRepository,
            IRepository<GLINVDetail> glinvDetailRepository,
            IRepository<AccountSubLedger> accountSubLedgerRepositor,
            IRepository<TaxClass, int> taxClassRepository)      
        {
            _glinvHeaderRepository = glinvHeaderRepository;
            _glinvDetailRepository = glinvDetailRepository;
            _accountSubLedgerRepository = accountSubLedgerRepositor;
            _taxClassRepository = taxClassRepository;
        }


        public List<SalesTaxDeductionReportDto> GetData(int? TenantId, string fromDate, string toDate, string fromAcc, string toAcc, string fromSubAcc, string toSubAcc, string taxAuth, string taxClass)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            var directInvoiceH = _glinvHeaderRepository.GetAll().Where(o => o.TenantId == TenantId && o.DocDate>= Convert.ToDateTime(fromDate) && o.DocDate <= Convert.ToDateTime(toDate)
            && taxAuth == o.TaxAuth && taxClass == o.TaxClass.ToString() && o.TypeID=="ST");
            var directInvoiceD = _glinvDetailRepository.GetAll().Where(o => o.TenantId == TenantId && o.AccountID.CompareTo(fromAcc) >= 0 && o.AccountID.CompareTo(toAcc) 
            <= 0 && o.SubAccID > 0 && o.SubAccID >= Convert.ToInt32(fromSubAcc) && o.SubAccID <= Convert.ToInt32(toSubAcc));
            var data = from a in directInvoiceH
                       join b in directInvoiceD
                       on new { A = a.Id } equals new { A = b.DetID }
                       join c in _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == TenantId)
                       on new { A = b.AccountID, B = Convert.ToInt32(b.SubAccID) } equals new { A = c.AccountID, B = c.Id }
                       join d in _taxClassRepository.GetAll().Where(o => o.TenantId == TenantId)
                       on new { A = Convert.ToInt32(a.TaxClass), B = a.TaxAuth } equals new { A = d.CLASSID, B = d.TAXAUTH }
                       orderby a.DocNo
                       select new SalesTaxDeductionReportDto()
                       {
                            Ntn = c.RegNo,
                            Cnic = c.CNICNO,
                            Name = c.SubAccName.ToString(),
                            DocNo = a.DocNo.ToString(),
                            DocDate = a.DocDate == null ? a.DocDate.ToString() : a.DocDate.ToString("dd/MM/yyyy"),
                            TaxRate = a.TaxRate.ToString(),
                            TaxClassDesc = d.CLASSDESC,
                            SalesValue = b.Amount.ToString(),
                            SalesTax =a.TaxAmount.ToString(),
                            StWithheld="0",
                            TotalSales=Convert.ToString(b.Amount+a.TaxAmount),
                       };
            return data.ToList();
        }
        
    }
}
