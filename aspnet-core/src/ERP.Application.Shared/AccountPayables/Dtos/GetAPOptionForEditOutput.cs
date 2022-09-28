using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.AccountPayables.Dtos
{
    public class GetAPOptionForEditOutput
    {
		public CreateOrEditAPOptionDto APOption { get; set; }

		public string CurrencyRateId { get; set;}

		public string BankBANKID { get; set;}

		public string ChartofControlId { get; set;}


    }
}