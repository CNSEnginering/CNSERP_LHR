using Abp.Application.Services.Dto;
using System;

namespace ERP.Payroll.SlabSetup.Dtos
{
    public class GetAllSlabSetupInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxTypeIDFilter { get; set; }
        public int? MinTypeIDFilter { get; set; }

        public double? MaxSlabFromFilter { get; set; }
        public double? MinSlabFromFilter { get; set; }

        public double? MaxSlabToFilter { get; set; }
        public double? MinSlabToFilter { get; set; }

        public double? MaxRateFilter { get; set; }
        public double? MinRateFilter { get; set; }

        public double? MaxAmountFilter { get; set; }
        public double? MinAmountFilter { get; set; }

        public int? ActiveFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }
        public DateTime? MinAudtDateFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }
        public DateTime? MinCreateDateFilter { get; set; }

        public string ModifiedByFilter { get; set; }

        public DateTime? MaxModifyDateFilter { get; set; }
        public DateTime? MinModifyDateFilter { get; set; }

    }
}