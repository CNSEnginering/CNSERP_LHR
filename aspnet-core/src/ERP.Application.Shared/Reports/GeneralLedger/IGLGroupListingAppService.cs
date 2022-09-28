using Abp.Application.Services;
using ERP.Reports.GeneralLedger.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.GeneralLedger
{
    public interface IGLGroupListingAppService : IApplicationService
    {
        List<GLGroupListingDto> GetData(int? TenantId, string fromCode, string toCode);
    }
}
