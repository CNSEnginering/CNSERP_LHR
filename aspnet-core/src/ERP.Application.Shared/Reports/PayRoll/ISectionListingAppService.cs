using Abp.Application.Services;
using ERP.Reports.PayRoll.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.PayRoll
{
    public interface ISectionListingAppService : IApplicationService
    {
        List<SectionListingDto> GetData(int? TenantId, string fromCode, string toCode, string description);
    }
}
