using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.SetupForms.LCExpenses.Dtos
{
    public class GetAllLCExpensesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MinExpIDFilter { get; set; }
        public int? MaxExpIDFilter { get; set; }
        public string ExpDescFilter { get; set; }
        public string AuditUserFilter { get; set; }
        public int ActiveFilter { get; set; }
        public DateTime? MaxAuditDateFilter { get; set; }

        public DateTime? MinAuditDateFilter { get; set; }
        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }

        public DateTime? MinCreateDateFilter { get; set; }

    }
}
