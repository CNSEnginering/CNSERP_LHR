using Abp.Application.Services;
using ERP.Reports.PayRoll.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.PayRoll
{
    public interface IGradeListingAppService : IApplicationService
    {
        List<GradeListingDto> GetData(int? TenantID, string fromCode, string toCode, string description);
    }
}
