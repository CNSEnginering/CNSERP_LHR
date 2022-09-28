using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.SalaryLock.Dtos
{
    public class SalaryLockDto : EntityDto
    {
        public int? TenantID { get; set; }

        public int? SalaryMonth { get; set; }

        public int? SalaryYear { get; set; }

        public bool Locked { get; set; }

        public DateTime? LockDate { get; set; }

    }
}