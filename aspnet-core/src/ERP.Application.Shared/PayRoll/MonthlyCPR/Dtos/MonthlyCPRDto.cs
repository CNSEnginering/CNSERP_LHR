using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.MonthlyCPR.Dtos
{
    public class MonthlyCPRDto : EntityDto
    {
        public short SalaryYear { get; set; }

        public short SalaryMonth { get; set; }

        public string CPRNumber { get; set; }

        public DateTime? CPRDate { get; set; }

        public double? Amount { get; set; }

        public string Remarks { get; set; }

        public bool Active { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}