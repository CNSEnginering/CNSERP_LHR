using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.Transaction.Dtos
{
    public class GetGlChequeForEditOutput
    {
		public CreateOrEditGlChequeDto GlCheque { get; set; }


    }
}