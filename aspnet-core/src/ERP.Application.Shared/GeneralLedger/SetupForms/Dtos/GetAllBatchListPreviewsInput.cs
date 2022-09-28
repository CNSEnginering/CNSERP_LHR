using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetAllBatchListPreviewsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int Status { get; set; }

        public string StatusFilter { get; set; }

        public string LocationFilter { get; set; }

        public string ReferenceFilter { get; set; }

        public string NarrationFilter { get; set; }

        public int? AmountFilter { get; set; }

        public int? maxVoucherNoFilter { get; set; }

        public int? minVoucherNoFilter { get; set; }

        public int? maxAmountFilter { get; set; }

        public int? minAmountFilter { get; set; }

        public string BookIDFilter { get; set; }

        public DateTime? MaxDocDateFilter { get; set; }

        public DateTime? MinDocDateFilter { get; set; }

        public int? MaxDocMonthFilter { get; set; }

        public int? MinDocMonthFilter { get; set; }

        public int PostedFilter { get; set; }

    }
}