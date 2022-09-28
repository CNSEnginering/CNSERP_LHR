using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.CommonServices.Dtos
{
    public class GetCompanyProfileForEditOutput
    {
		public CreateOrEditCompanyProfileDto CompanyProfile { get; set; }


    }
}