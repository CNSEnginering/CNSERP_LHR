using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.DeductionTypes.Dtos
{
    public class GetDeductionTypesForEditOutput
    {
		public CreateOrEditDeductionTypesDto DeductionTypes { get; set; }


    }
}