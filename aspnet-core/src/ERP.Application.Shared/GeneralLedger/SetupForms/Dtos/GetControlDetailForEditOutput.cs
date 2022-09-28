using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetControlDetailForEditOutput
    {
		public CreateOrEditControlDetailDto ControlDetail { get; set; }

		public int Family { get; set;}


    }
}