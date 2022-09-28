using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.LedgerType.Dtos
{
    public class GetLedgerTypeForEditOutput
    {
		public CreateOrEditLedgerTypeDto LedgerType { get; set; }


    }
}