using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.Transaction.BankReconcile.Dtos
{
    public class GetBankReconcileForEditOutput
    {
		public CreateOrEditBankReconcileDto BankReconcile { get; set; }


    }
}