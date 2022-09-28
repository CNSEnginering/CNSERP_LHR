using Abp.Application.Services;
using ERP.Reports.GeneralLedger.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.GeneralLedger
{
    public interface IGLConfigListingAppService : IApplicationService
    {
        List<GLConfigListingDto> GetData(int? TenantId, string fromCode, string toCode);
    }
}
