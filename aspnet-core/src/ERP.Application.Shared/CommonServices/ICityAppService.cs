using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.CommonServices.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.CommonServices
{
    public interface ICityAppService : IApplicationService
    {
        Task<PagedResultDto<GetCityForViewDto>> GetAll();

    }
}
