using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Sales.Dtos
{
    public class GetCreditDebitNoteForEditOutput
    {
		public CreateOrEditCreditDebitNoteDto CreditDebitNote { get; set; }

    }
}