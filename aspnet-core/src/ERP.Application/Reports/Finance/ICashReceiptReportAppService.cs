using Abp.Application.Services;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.Reports.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Reports.Finance
{
    public interface ICashReceiptReportAppService : IApplicationService
    {
        GLOption GetSignatures(int tenantId);
        double? GetDebitSum(int tenantId, int docNo, string bookId, int docMonth);
        List<CashReceiptModel> GetCashReceipt(int? tenantId, string bookId, int? year, int? month, int fromConfigId, int toConfigId, int fromDoc, int toDoc, int locId, string curId, double? curRate, string status);
    }
}
