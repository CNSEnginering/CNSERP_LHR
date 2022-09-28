using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.AllowanceSetup.Dtos
{
    public class GetAllowanceSetupForEditOutput
    {
		public CreateOrEditAllowanceSetupDto AllowanceSetup { get; set; }


    }
}