using Abp.Application.Services;
using ERP.Reports.GeneralLedger.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.GeneralLedger
{
    public interface IGLLevel2ListingAppService : IApplicationService
    {
        List<GLLevel2ListingDto> GetData(int? TenantId, string fromCode, string toCode);
    }
}
