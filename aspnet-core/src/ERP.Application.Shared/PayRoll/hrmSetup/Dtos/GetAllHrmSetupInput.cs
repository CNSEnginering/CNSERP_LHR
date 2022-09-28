using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.hrmSetup.Dtos
{
    public class GetAllHrmSetupInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string AdvToStSalFilter { get; set; }

        public string AdvToPayableFilter { get; set; }

        public string LoanToStSalFilter { get; set; }

        public string LoanToPayableFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }
        public DateTime? MinCreateDateFilter { get; set; }

        public DateTime? MaxAuditDateFilter { get; set; }
        public DateTime? MinAuditDateFilter { get; set; }

        public string AuditByFilter { get; set; }

    }
}