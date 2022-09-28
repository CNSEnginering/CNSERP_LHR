using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetGroupCodeForEditOutput
    {
		public CreateOrEditGroupCodeDto GroupCode { get; set; }

		public int GRPCTCODE { get; set;}


    }
}