using ERP.Reports.Finance.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.Finance
{
    public interface ILCExpensesReportAppService
    {
        List<LCExpensesReportDto> GetData(int? TenantId, string fromDate, string toDate, string fromCode, string toCode);
    }
}
