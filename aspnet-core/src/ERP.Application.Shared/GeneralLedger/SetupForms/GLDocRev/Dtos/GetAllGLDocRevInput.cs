using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.SetupForms.GLDocRev.Dtos
{
    public class GetAllGLDocRevInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string BookIDFilter { get; set; }

        public int? MaxDocNoFilter { get; set; }
        public int? MinDocNoFilter { get; set; }

        public DateTime? MaxDocDateFilter { get; set; }
        public DateTime? MinDocDateFilter { get; set; }

        public int? MaxFmtDocNoFilter { get; set; }
        public int? MinFmtDocNoFilter { get; set; }

        public int? MaxDocYearFilter { get; set; }
        public int? MinDocYearFilter { get; set; }

        public int? MaxDocMonthFilter { get; set; }
        public int? MinDocMonthFilter { get; set; }

        public DateTime? MaxNewDocDateFilter { get; set; }
        public DateTime? MinNewDocDateFilter { get; set; }

        public int? MaxNewDocNoFilter { get; set; }
        public int? MinNewDocNoFilter { get; set; }

        public int? MaxNewFmtDocNoFilter { get; set; }
        public int? MinNewFmtDocNoFilter { get; set; }

        public int? MaxNewDocYearFilter { get; set; }
        public int? MinNewDocYearFilter { get; set; }

        public int? MaxNewDocMonthFilter { get; set; }
        public int? MinNewDocMonthFilter { get; set; }

        public string NarrationFilter { get; set; }

        public int? PostedFilter { get; set; }

        public string PostedByFilter { get; set; }

        public DateTime? MaxPostedDateFilter { get; set; }
        public DateTime? MinPostedDateFilter { get; set; }

    }
}