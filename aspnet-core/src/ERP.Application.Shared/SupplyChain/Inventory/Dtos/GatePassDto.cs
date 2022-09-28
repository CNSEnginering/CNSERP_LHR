
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class GatePassDto : EntityDto
    {
		public short TypeID { get; set; }

		public int DocNo { get; set; }

		public DateTime? DocDate { get; set; }

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



    }
}