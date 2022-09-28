
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Sales.SalesReference.Dtos
{
    public class SalesReferenceDto : EntityDto
    {
        public string RefType { get; set; }
        public int RefID { get; set; }

		public string RefName { get; set; }

		public bool ACTIVE { get; set; }

		public DateTime? AUDTDATE { get; set; }

		public string AUDTUSER { get; set; }

		public DateTime? CreatedDATE { get; set; }

		public string CreatedUSER { get; set; }



    }
}