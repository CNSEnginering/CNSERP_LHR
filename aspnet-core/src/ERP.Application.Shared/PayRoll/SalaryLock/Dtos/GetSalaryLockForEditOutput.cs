using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.SalaryLock.Dtos
{
    public class GetSalaryLockForEditOutput
    {
        public CreateOrEditSalaryLockDto SalaryLock { get; set; }

    }
}