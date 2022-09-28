using Abp.Application.Services.Dto;
using System;

namespace ERP.CommonServices.Dtos
{
    public class GetAllCompanyProfilesForExcelInput
    {
		public string Filter { get; set; }

		public string CompanyNameFilter { get; set; }

		public string PhoneFilter { get; set; }



    }
}