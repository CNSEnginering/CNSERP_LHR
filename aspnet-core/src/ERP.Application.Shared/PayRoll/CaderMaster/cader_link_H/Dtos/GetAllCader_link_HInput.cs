using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.CaderMaster.cader_link_H.Dtos
{
    public class GetAllCader_link_HInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CreatedbyFilter { get; set; }

        public DateTime? MaxCreatedDateFilter { get; set; }
        public DateTime? MinCreatedDateFilter { get; set; }

        public DateTime? MaxAuditdateFilter { get; set; }
        public DateTime? MinAuditdateFilter { get; set; }

        public string Audit_byFilter { get; set; }

    }
}