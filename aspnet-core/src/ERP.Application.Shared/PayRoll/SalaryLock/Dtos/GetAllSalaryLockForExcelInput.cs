using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.SalaryLock.Dtos
{
    public class GetAllSalaryLockForExcelInput
    {
        public string Filter { get; set; }

        public int? MaxTenantIDFilter { get; set; }
        public int? MinTenantIDFilter { get; set; }

        public int? MaxSalaryMonthFilter { get; set; }
        public int? MinSalaryMonthFilter { get; set; }

        public int? MaxSalaryYearFilter { get; set; }
        public int? MinSalaryYearFilter { get; set; }

        public int? LockFilter { get; set; }

        public DateTime? MaxLockDateFilter { get; set; }
        public DateTime? MinLockDateFilter { get; set; }

    }
}