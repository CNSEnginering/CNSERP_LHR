
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.LCExpenses.Dtos
{
    public class LCExpensesDto : EntityDto
    {
        public int ExpID { get; set; }
        public string ExpDesc { get; set; }
        public bool Active { get; set; }
        public string AuditUser { get; set; }
        public DateTime? AuditDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}