using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.hrmSetup.Dtos
{
    public class HrmSetupDto : EntityDto
    {
        public string AdvToStSal { get; set; }

        public string AdvToPayable { get; set; }

        public string LoanToStSal { get; set; }

        public string LoanToPayable { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? AuditDate { get; set; }

        public string AuditBy { get; set; }
        public string AdvToStSalName { get; set; }

        public string AdvToPayablName { get; set; }

        public string LoanToStSalName { get; set; }

        public string LoanToPayableName { get; set; }

    }
}