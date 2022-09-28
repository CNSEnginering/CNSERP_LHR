using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.CommonServices.Dtos
{
    public class GetTaxClassForEditOutput
    {
		public CreateOrEditTaxClassDto TaxClass { get; set; }

		public string TaxAuthorityTAXAUTH { get; set;}
        public string TaxAccDesc { get; set; }

    }
}