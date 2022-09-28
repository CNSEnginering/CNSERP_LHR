
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class ICSetupDto : EntityDto<int?>
    {
		public string Segment1 { get; set; }

		public string Segment2 { get; set; }

		public string Segment3 { get; set; }
		public string Currency { get; set; }

		public bool AllowNegative { get; set; }

		public int? ErrSrNo { get; set; }
		public int? InventoryPoint { get; set; }
		public string TransType { get; set; }
		public int? DamageLocID { get; set; }
		public string DamageLocName { get; set; }
		public string TransTypeName { get; set; }
		public double? CURRATE { get; set; }

		public int? CostingMethod { get; set; }

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

		public bool SalesReturnLinkOn { get; set; }

		public bool SalesLinkOn { get; set; }

		public bool AccLinkOn { get; set; }

        public int? conType { get; set; }

        public int? CurrentLocID { get; set; }
		public string CurrentLocName { get; set; }

		public int? GLSegLink { get; set; }

		public bool AllowLocID { get; set; }

		public bool CDateOnly { get; set; }

		public string Opt4 { get; set; }

		public string Opt5 { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateadOn { get; set; }



    }
}