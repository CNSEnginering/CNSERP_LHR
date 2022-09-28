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
using ERP.GeneralLedger.SetupForms.LCExpensesHD;

namespace ERP.Reports.Finance
{
    public class LCExpensesReportAppService : ERPReportAppServiceBase , ILCExpensesReportAppService
    {
        private readonly IRepository<LCExpensesHeader> _lcExpensesHeaderRepository;
        private readonly IRepository<LCExpensesDetail> _lcExpensesDetailRepository;
        public LCExpensesReportAppService(
            IRepository<LCExpensesHeader> lcExpensesHeaderRepository,
            IRepository<LCExpensesDetail> lcExpensesDetailRepository)      
        {
            _lcExpensesHeaderRepository = lcExpensesHeaderRepository;
            _lcExpensesDetailRepository = lcExpensesDetailRepository;
        }


        public List<LCExpensesReportDto> GetData(int? TenantId, string fromDate, string toDate, string fromCode, string toCode)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            var lcExpensesH = _lcExpensesHeaderRepository.GetAll().Where(o => o.TenantId == TenantId && o.DocDate>= Convert.ToDateTime(fromDate) && o.DocDate <= Convert.ToDateTime(toDate).AddDays(1)
            && o.SubAccID >=Convert.ToInt32(fromCode) && o.SubAccID <= Convert.ToInt32(toCode));
            var lcExpensesD = _lcExpensesDetailRepository.GetAll().Where(o => o.TenantId == TenantId);
            var data = from a in lcExpensesH
                       join b in lcExpensesD
                       on new { A = a.Id } equals new { A = b.DetID }
                       orderby a.DocNo

                       select new LCExpensesReportDto
                       {
                            VoucherDate = a.DocDate == null ? a.DocDate.ToString() : a.DocDate.Value.ToString("dd/MM/yyyy"),
                            VoucherNo = a.DocNo.ToString(),
                            LCNo = a.LCNumber,
                            SLCode = a.SubAccID.ToString(),
                            LCExpDesc = b.ExpDesc,
                            Amount = Convert.ToInt64(b.Amount)
                       };
            return data.ToList();
        }
        
    }
}
