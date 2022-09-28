using Abp.Application.Services;
using ERP.Reports.PayRoll.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using ERP.Reports.PayRoll;

namespace ERP.Reports.PayRoll
{
    public interface ITaxCertificateAppService : IApplicationService
    {
        List<TaxCertificate> GetData(int? TenantId, string fromEmpID, short SalaryYear, short SalaryMonth, short toSalaryYear, short toSalaryMonth);
    }
}
