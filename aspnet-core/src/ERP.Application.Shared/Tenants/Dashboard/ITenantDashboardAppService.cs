using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Reports;
using ERP.Tenants.Dashboard.Dto;
using System;
using System.Threading.Tasks;

namespace ERP.Tenants.Dashboard
{
    public interface ITenantDashboardAppService : IApplicationService
    {
        GetMemberActivityOutput GetMemberActivity();

        GetDashboardDataOutput GetDashboardData(GetDashboardDataInput input);

        GetSalesSummaryOutput GetSalesSummary(GetSalesSummaryInput input);

        GetRegionalStatsOutput GetRegionalStats();

        GetGeneralStatsOutput GetGeneralStats();

        Task<ListResultDto<CashFlowSummary>> CashFlowSummary(GetCashFlowSummaryInput input);

        Task<ListResultDto<BankStatusSummary>> BankStatusSummary(GetBankStatusSummaryInput input);
    }
}
