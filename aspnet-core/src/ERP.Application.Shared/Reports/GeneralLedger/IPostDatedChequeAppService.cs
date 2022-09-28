using Abp.Application.Services;
using ERP.Reports.GeneralLedger.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.GeneralLedger
{
    public interface IPostDatedChequeAppService : IApplicationService
    {
        List<PostDatedChequeDto> GetData(int? TenantId, string fromCode, string toCode, string fromDate, string toDate, int? typeID);
    }
}
