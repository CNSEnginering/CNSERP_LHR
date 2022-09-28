using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Dto;
using ERP.PayRoll.Location.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.PayRoll.Location
{
    public interface ILocationAppService : IApplicationService
    {
        Task<PagedResultDto<GetLocationForViewDto>> GetAll(GetAllLocationInput input);

        Task<GetLocationForViewDto> GetLocationForView(int id);

        Task<GetLocationForEditOutput> GetLocationForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditLocationDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetLocationToExcel(GetAllLocationForExcelInput input);
    }
}
