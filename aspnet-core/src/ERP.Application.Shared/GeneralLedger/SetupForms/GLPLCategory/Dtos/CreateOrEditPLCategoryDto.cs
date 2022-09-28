
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.GLPLCategory.Dtos
{
    public class CreateOrEditPLCategoryDto : EntityDto<int?>
    {

		public int? TenantID { get; set; }
		
		
		[StringLength(PLCategoryConsts.MaxTypeIDLength, MinimumLength = PLCategoryConsts.MinTypeIDLength)]
		public string TypeID { get; set; }
		
		
		[StringLength(PLCategoryConsts.MaxHeadingTextLength, MinimumLength = PLCategoryConsts.MinHeadingTextLength)]
		public string HeadingText { get; set; }
		
		
		public short? SortOrder { get; set; }
		
		

    }
}