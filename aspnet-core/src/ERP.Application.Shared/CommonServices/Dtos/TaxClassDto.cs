
using System;
using Abp.Application.Services.Dto;

namespace ERP.CommonServices.Dtos
{
    public class TaxClassDto : EntityDto
    {
		public string TAXAUTH { get; set; }

		public string CLASSDESC { get; set; }

		public double? CLASSRATE { get; set; }

		public double? TRANSTYPE { get; set; }

		public string CLASSTYPE { get; set; }

        public string TAXACCID { get; set; }

        public string TAXACCDESC { get; set; }

        public bool IsActive { get; set; }

		public DateTime? AUDTDATE { get; set; }

		public string AUDTUSER { get; set; }


		// public string? TaxAuthorityId { get; set; }

		 
    }
}