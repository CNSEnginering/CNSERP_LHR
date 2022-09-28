using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.CaderMaster.cader_link_D.Dtos
{
    public class GetAllCader_link_DInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxCaderIDFilter { get; set; }
        public int? MinCaderIDFilter { get; set; }

        public string AccountIDFilter { get; set; }

        public string AccountNameFilter { get; set; }

        public string TypeFilter { get; set; }

        public string PayTypeFilter { get; set; }

        public string NarrationFilter { get; set; }

    }
}