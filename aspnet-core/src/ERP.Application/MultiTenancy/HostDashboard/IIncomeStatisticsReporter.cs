using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ERP.MultiTenancy.HostDashboard.Dto;

namespace ERP.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}