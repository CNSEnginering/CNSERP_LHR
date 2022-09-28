using ERP.Reports.Finance.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.Finance
{
    public interface IBankReconcilationReportAppService
    {
        List<BankReconcilationReportDto> GetData(int? TenantId, string bankID, DateTime fromDate, DateTime toDate);
        List<BankReconcilationReportDetailDto> GetDetailData(int? TenantId, string bankID, DateTime fromDate, DateTime toDate);
    }
}
