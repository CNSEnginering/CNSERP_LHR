
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Sales.SalesReference.Dtos
{
    public class CreateOrEditSalesReferenceDto : EntityDto<int?>
    {

		[Required]
		public int RefID { get; set; }

        public string RefType { get; set; }
        public string RefName { get; set; }
		
		
		public bool ACTIVE { get; set; }
		
		
		public DateTime? AUDTDATE { get; set; }
		
		
		public string AUDTUSER { get; set; }
		
		
		public DateTime? CreatedDATE { get; set; }
		
		
		public string CreatedUSER { get; set; }
		
		

    }
}