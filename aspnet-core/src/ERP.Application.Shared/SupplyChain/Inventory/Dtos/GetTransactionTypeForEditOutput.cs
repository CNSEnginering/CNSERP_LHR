using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class GetTransactionTypeForEditOutput
    {
		public CreateOrEditTransactionTypeDto TransactionType { get; set; }


    }
}