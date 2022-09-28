using ERP.Reports.Finance.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.Finance
{
    public interface IBankReconcileReportAppService
    {
        List<BankReconcileReportDto> GetData(int? TenantId, string bankID, string fromDocID, string toDocID);
    }
}
