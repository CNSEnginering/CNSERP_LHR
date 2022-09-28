using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.LCExpenses.Dtos
{
    public class CreateOrEditLCExpensesDto : EntityDto<int?>
    {
        [Required]
        public int ExpID { get; set; }
        [Required]
        public string ExpDesc { get; set; }
        public bool Active { get; set; }
        public string AuditUser { get; set; }
        public DateTime? AuditDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
