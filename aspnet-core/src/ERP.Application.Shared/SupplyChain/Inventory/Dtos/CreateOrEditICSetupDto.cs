
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class CreateOrEditICSetupDto : EntityDto<int?>
    {

		public string Segment1 { get; set; }
		
		
		public string Segment2 { get; set; }
		
		
		public string Segment3 { get; set; }
		
		
		public int? AllowNegative { get; set; }
		
		
		public int? ErrSrNo { get; set; }
		
		
		public short? CostingMethod { get; set; }
		
		
		public string PRBookID { get; set; }
		
		
		public string RTBookID { get; set; }
		
		
		public string CnsBookID { get; set; }
		
		
		public string SLBookID { get; set; }
		
		
		public string SRBookID { get; set; }
		
		
		public string TRBookID { get; set; }
		
		
		public string PrdBookID { get; set; }
		
		
		public string PyRecBookID { get; set; }
		
		
		public string AdjBookID { get; set; }
		
		
		public string AsmBookID { get; set; }
		
		
		public string WSBookID { get; set; }
		
		
		public string DSBookID { get; set; }
		
		
		public short? SalesReturnLinkOn { get; set; }
		
		
		public short? SalesLinkOn { get; set; }
		
		
		public short? AccLinkOn { get; set; }
		
		
		public int? CurrentLocID { get; set; }
		
		
		public short? AllowLocID { get; set; }
		
		
		public short? CDateOnly { get; set; }
		
		
		public string Opt4 { get; set; }
		
		
		public string Opt5 { get; set; }
		
		
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateadOn { get; set; }
		
		

    }
}