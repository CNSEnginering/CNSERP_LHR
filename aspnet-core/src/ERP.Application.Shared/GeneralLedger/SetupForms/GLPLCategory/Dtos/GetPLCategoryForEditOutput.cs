using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.GLPLCategory.Dtos
{
    public class GetPLCategoryForEditOutput
    {
		public CreateOrEditPLCategoryDto PLCategory { get; set; }


    }
}