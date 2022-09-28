
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using ERP.SupplyChain.Inventory;
namespace ERP.SupplyChain.Inventory.Dtos
{
    public class CreateOrEditGatePassDto : EntityDto<int?>
    {

		[Required]
		public short TypeID { get; set; }
		
		
		[Required]
		public int DocNo { get; set; }
		
		
		public string DocDate { get; set; }
		
		
		public string AccountID { get; set; }

        public string AccountName { get; set; }
        public int? PartyID { get; set; }

        public string PartyName { get; set; }
        public string Narration { get; set; }
		
		
		public short? GPType { get; set; }
		
		
		public string DriverName { get; set; }
		
		
		public string VehicleNo { get; set; }
		
		
		public int? GPDocNo { get; set; }
		
		
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
        public string OrderNo { get; set; }
        public List<GatePassDetailDto> gatePassDetailDto { get; set; }

        public virtual string DCNo { get; set; }



    }
}