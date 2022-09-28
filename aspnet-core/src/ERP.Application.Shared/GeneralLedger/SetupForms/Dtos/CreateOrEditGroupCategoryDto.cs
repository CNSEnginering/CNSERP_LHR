
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class CreateOrEditGroupCategoryDto : EntityDto<int?>
    {
        public int GRPCTCODE { get; set; }
        public string GRPCTDESC { get; set; }
		
		

    }
}