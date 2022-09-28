using Abp.Application.Services;
using ERP.Reports.GeneralLedger.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.GeneralLedger
{
    public interface IGLLevel3ListingAppService : IApplicationService
    {
        List<GLLevel3ListingDto> GetData(int? TenantId, string fromCode, string toCode);
    }
}
