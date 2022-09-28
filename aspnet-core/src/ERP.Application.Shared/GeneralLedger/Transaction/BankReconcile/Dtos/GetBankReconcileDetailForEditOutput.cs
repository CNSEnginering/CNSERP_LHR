using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERP.GeneralLedger.Transaction.BankReconcile.Dtos
{
    public class GetBankReconcileDetailForEditOutput
    {
		public ICollection<CreateOrEditBankReconcileDetailDto> BankReconcileDetail { get; set; }


    }
}