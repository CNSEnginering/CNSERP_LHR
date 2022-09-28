using Abp.Application.Services;
using ERP.Reports.Finance.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Reports.Finance
{
    public interface IProfitAndLossStatment : IApplicationService
    {
        List<ProfitAndLossStatmentDto> GetProfitAndLossStatment(DateTime FromDate, DateTime ToDate, int loc);
    }
}
