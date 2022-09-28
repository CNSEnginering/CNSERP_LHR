using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetGLOptionForEditOutput
    {
		public CreateOrEditGLOptionDto GLOption { get; set; }

		public string ChartofControlId { get; set;}


    }
}