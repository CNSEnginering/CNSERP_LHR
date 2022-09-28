using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Reports
{
    public interface ICustomerAgingReportAppService : IApplicationService
    {
        List<CustomerAgingDto> GetAll(CustomerAgingInputs inputs);
    }
}
