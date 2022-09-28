
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Sales.SaleAccounts.Dtos
{
    public class OECOLLDto : EntityDto
    {
		public int LocID { get; set; }
        public string LocName { get; set; }
        public string TypeID { get; set; }
        public string TypeName { get; set; }
        public string SalesACC { get; set; }

		public string SalesRetACC { get; set; }

		public string COGSACC { get; set; }

		public string ChAccountID { get; set; }

		public string DiscAcc { get; set; }

		public string WriteOffAcc { get; set; }

		public short? Active { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }



    }
}