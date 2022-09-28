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
using ERP.GeneralLedger.Transaction.BankReconcile;

namespace ERP.Reports.Finance
{
    public class BankReconcileReportAppService : ERPReportAppServiceBase , IBankReconcileReportAppService
    {
        private readonly IRepository<BankReconcile> _bankReconcileHeaderRepository;
        private readonly IRepository<BankReconcileDetail> _bankReconcileDetailRepository;
        public BankReconcileReportAppService(
            IRepository<BankReconcile> bankReconcileHeaderRepository,
            IRepository<BankReconcileDetail> bankReconcileDetailRepository
            )
        {
            _bankReconcileHeaderRepository = bankReconcileHeaderRepository;
            _bankReconcileDetailRepository = bankReconcileDetailRepository;
        }

      
        public List<BankReconcileReportDto> GetData(int? TenantId, string bankID, string fromDocID, string toDocID)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            var bankHeader = _bankReconcileHeaderRepository.GetAll().Where(o => o.TenantId == TenantId &&
            o.BankID == bankID && SplitMethod(o.DocID).CompareTo(fromDocID)>=0 && SplitMethod(o.DocID).CompareTo(toDocID) <= 0);

            var data = from a in bankHeader
                       join b in _bankReconcileDetailRepository.GetAll().Where(o=>o.TenantId==TenantId) 
                       on new { A=a.TenantId, B=a.Id } equals new { A=b.TenantId, B=b.DetID}
                       orderby a.DocID
                       select new BankReconcileReportDto()
                       {
                          DocID = a.DocID,
                          DocDate = a.DocDate == null ? a.DocDate.ToString() : a.DocDate.Value.ToString("dd/MM/yyyy"),
                          BankID = a.BankID,
                          BankName= a.BankName,
                          BeginBalance = a.BeginBalance.ToString(),
                          EndBalance = a.EndBalance.ToString(),
                          ReconcileAmt = a.ReconcileAmt.ToString(),
                          DiffAmount = a.DiffAmount.ToString(),
                          BookID= b.BookID + "-" + b.ConfigID.ToString(),
                          VoucherDate = b.VoucherDate == null ? b.VoucherDate.ToString() : b.VoucherDate.Value.ToString("dd/MM/yyyy"),
                          ClearingDate = b.ClearingDate == null ? b.ClearingDate.ToString() : b.ClearingDate.Value.ToString("dd/MM/yyyy"),
                          VoucherID = b.VoucherID.ToString(),
                          Dr = b.Amount > 0 ? b.Amount.ToString() : "0",
                          Cr = b.Amount < 0 ? b.Amount.ToString() : "0",
                          Include = b.Include ==true ? "Yes":"No",

                       }; return data.ToList();
        }

        public string SplitMethod(string str)
        {
            return str.Split("-")[1];
        }
    }
   
    //public class UserDTO
    //{
    //    public int ID { get; set; }

    //    public string Name { get; set; }
    //}
}
