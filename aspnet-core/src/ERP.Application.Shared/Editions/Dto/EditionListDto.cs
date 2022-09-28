using Abp.Application.Services.Dto;

namespace ERP.Editions.Dto
{
    public class EditionListDto : EntityDto
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public decimal? MonthlyPrice { get; set; }

        public decimal? AnnualPrice { get; set; }

        public int? WaitingDayAfterExpire { get; set; }

        public int? TrialDayCount { get; set; }

        public string ExpiringEditionDisplayName { get; set; }
    }
}