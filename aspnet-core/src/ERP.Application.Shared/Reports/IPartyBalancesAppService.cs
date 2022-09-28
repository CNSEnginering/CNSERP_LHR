using ERP.Reports.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Reports
{
    public interface IPartyBalancesAppService
    {
        List<ConsolidatedLedgers> GetConsolidatedLedgers(DateTime FromDate, DateTime ToDate, string FromAccount, string ToAccount, int? FromSubAccID, int? ToSubAccID, int TenantId);
    }
}
