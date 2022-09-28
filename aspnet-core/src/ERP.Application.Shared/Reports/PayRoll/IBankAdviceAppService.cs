using Abp.Application.Services;
using ERP.Reports.PayRoll.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.PayRoll
{
    public interface IBankAdviceAppService : IApplicationService
    {
        List<BankAdviceDto> GetData(int? TenantId, short SalaryMonth, short SalaryYear, int typeID);
    }
}
