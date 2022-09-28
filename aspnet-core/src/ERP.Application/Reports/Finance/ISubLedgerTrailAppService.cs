using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Reports.Finance
{
    public interface ISubLedgerTrailAppService : IApplicationService
    {

        List<SubLedgerTrial> GetAll(DateTime FromDate, DateTime ToDate, string FromAccountID, string ToAccountID, int? FromSubAccID, int? ToSubAccID, int? SLType, int? TenantID, string Status, int? curRate);
    }
}
