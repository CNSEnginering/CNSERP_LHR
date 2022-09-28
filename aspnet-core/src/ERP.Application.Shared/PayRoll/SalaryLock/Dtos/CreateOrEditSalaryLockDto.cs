using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.SalaryLock.Dtos
{
    public class CreateOrEditSalaryLockDto : EntityDto<int?>
    {

        public int? TenantID { get; set; }

        public int? SalaryMonth { get; set; }

        public int? SalaryYear { get; set; }

        public bool Locked { get; set; }
        public bool JVLocked { get; set; }

        public DateTime? LockDate { get; set; }

    }
}