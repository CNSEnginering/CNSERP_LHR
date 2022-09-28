using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.Holidays.Dtos
{
    public class GetHolidaysForEditOutput
    {
		public CreateOrEditHolidaysDto Holidays { get; set; }


    }
}