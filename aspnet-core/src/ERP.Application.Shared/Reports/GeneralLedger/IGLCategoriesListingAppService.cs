using Abp.Application.Services;
using ERP.Reports.GeneralLedger.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.GeneralLedger
{
    public interface IGLCategoriesListingAppService : IApplicationService
    {
        List<GLCategoriesListingDto> GetData(int? TenantId, string fromCode, string toCode);
    }
}
