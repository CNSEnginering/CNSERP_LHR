
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class CreateOrEditGLBSCtgDto : EntityDto<int?>
    {

		public int? TenantID { get; set; }
		
		
		[StringLength(GLBSCtgConsts.MaxBSTypeLength, MinimumLength = GLBSCtgConsts.MinBSTypeLength)]
		public string BSType { get; set; }
		
		
		[StringLength(GLBSCtgConsts.MaxBSAccTypeLength, MinimumLength = GLBSCtgConsts.MinBSAccTypeLength)]
		public string BSAccType { get; set; }
		
		
		[StringLength(GLBSCtgConsts.MaxBSAccDescLength, MinimumLength = GLBSCtgConsts.MinBSAccDescLength)]
		public string BSAccDesc { get; set; }
		
		
		public int? SortOrder { get; set; }
		
		
		public int? BSGID { get; set; }
		
		

    }
}