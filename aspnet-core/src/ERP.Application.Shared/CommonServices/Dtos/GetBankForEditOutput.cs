using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.CommonServices.Dtos
{
    public class GetBankForEditOutput
    {
		public CreateOrEditBankDto Bank { get; set; }

		public string ChartofControlId { get; set;}


    }
}