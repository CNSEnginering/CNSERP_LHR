using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.EarningTypes.Dtos
{
    public class GetEarningTypesForEditOutput
    {
		public CreateOrEditEarningTypesDto EarningTypes { get; set; }


    }
}