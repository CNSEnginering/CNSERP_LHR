using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using System.Collections.Generic;

namespace ERP.GeneralLedger.SetupForms
{
    public interface IVendorActivityAppService : IApplicationService 
    {
        ListResultDto<GetVenderActivityForViewDto> GetVendorActivityForView(GetVendorActivityInputs inputs);

		
		
    }
}