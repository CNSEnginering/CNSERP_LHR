using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms
{
	[Table("GLBSCtg")]
    public class GLBSCtg : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual int? TenantID { get; set; }
		
		[StringLength(GLBSCtgConsts.MaxBSTypeLength, MinimumLength = GLBSCtgConsts.MinBSTypeLength)]
		public virtual string BSType { get; set; }
		
		[StringLength(GLBSCtgConsts.MaxBSAccTypeLength, MinimumLength = GLBSCtgConsts.MinBSAccTypeLength)]
		public virtual string BSAccType { get; set; }
		
		[StringLength(GLBSCtgConsts.MaxBSAccDescLength, MinimumLength = GLBSCtgConsts.MinBSAccDescLength)]
		public virtual string BSAccDesc { get; set; }
		
		public virtual int? SortOrder { get; set; }
		
		public virtual int? BSGID { get; set; }
		

    }
}