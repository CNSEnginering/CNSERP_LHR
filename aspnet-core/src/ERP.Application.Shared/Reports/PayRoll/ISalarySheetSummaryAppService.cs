using Abp.Application.Services;
using ERP.Reports.PayRoll.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.PayRoll
{
    public interface ISalarySheetSummaryAppService : IApplicationService
    {
        List<SalarySheetSummaryDto> GetData(int? TenantId, short SalaryMonth, short SalaryYear);
    }
}
