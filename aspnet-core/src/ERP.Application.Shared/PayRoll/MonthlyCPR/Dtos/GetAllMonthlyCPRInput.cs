using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.MonthlyCPR.Dtos
{
    public class GetAllMonthlyCPRInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public short? MaxSalaryYearFilter { get; set; }
        public short? MinSalaryYearFilter { get; set; }

        public short? MaxSalaryMonthFilter { get; set; }
        public short? MinSalaryMonthFilter { get; set; }

        public string CPRNumberFilter { get; set; }

        public DateTime? MaxCPRDateFilter { get; set; }
        public DateTime? MinCPRDateFilter { get; set; }

        public double? MaxAmountFilter { get; set; }
        public double? MinAmountFilter { get; set; }

        public string RemarksFilter { get; set; }

        public int? ActiveFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }
        public DateTime? MinAudtDateFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }
        public DateTime? MinCreateDateFilter { get; set; }

    }
}