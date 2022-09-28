using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms.GLPLCategory
{
	[Table("GLPLCtg")]
    public class PLCategory : Entity , IMustHaveTenant
    {

        [Column("TenantID")]
		public int TenantId { get; set; }
		
		[StringLength(PLCategoryConsts.MaxTypeIDLength, MinimumLength = PLCategoryConsts.MinTypeIDLength)]
		public virtual string TypeID { get; set; }
		
		[StringLength(PLCategoryConsts.MaxHeadingTextLength, MinimumLength = PLCategoryConsts.MinHeadingTextLength)]
		public virtual string HeadingText { get; set; }
		
		public virtual short? SortOrder { get; set; }
        public virtual int? GLPLCtGId { get; set; }

    }
}