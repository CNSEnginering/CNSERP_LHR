using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.Transaction.Dtos
{
    public class GetAccountsPostingForEditOutput
    {
		public CreateOrEditAccountsPostingDto AccountsPosting { get; set; }


    }
}