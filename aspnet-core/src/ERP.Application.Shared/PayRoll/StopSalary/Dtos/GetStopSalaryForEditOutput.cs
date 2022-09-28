using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.StopSalary.Dtos
{
    public class GetStopSalaryForEditOutput
    {
		public CreateOrEditStopSalaryDto StopSalary { get; set; }


    }
}