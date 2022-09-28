using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.AccountPayables.Setup.ApSetup.Dtos
{
    public class GetApSetupForEditOutput
    {
		public CreateOrEditApSetupDto ApSetup { get; set; }


    }
}