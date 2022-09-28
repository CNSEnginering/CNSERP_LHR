using Abp.Application.Services;
using ERP.Reports.GeneralLedger.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.GeneralLedger
{
    public interface ILedgerTypeListingAppService : IApplicationService
    {
        List<LedgerTypeListingDto> GetData(int? TenantId, string fromCode, string toCode);
    }
}
