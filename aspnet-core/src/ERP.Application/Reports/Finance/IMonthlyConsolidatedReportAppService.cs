using Abp.Application.Services;
using ERP.Reports.Finance.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.Finance
{
    public interface IMonthlyConsolidatedReportAppService : IApplicationService
    {
        List<MonthlyConsolidatedRpt> GetConsolidatedLedgers(DateTime fromDate, DateTime toDate, string fromAccount, string toAccount, string status, int fromLocId, int toLocId, int? curRate);
    }
}
