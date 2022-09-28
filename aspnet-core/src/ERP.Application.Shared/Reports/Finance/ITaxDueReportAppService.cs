﻿using Abp.Application.Services;
using ERP.Reports.Finance.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.Finance
{
    public interface ITaxDueReportAppService : IApplicationService
    {
        List<TaxDueReportDto> GetData(int? TenantId,string fromDate, string toDate, string fromAcc, string toAcc, string fromSubAcc, string toSubAcc, string taxAuth, string taxClass, string type);

    }
}
