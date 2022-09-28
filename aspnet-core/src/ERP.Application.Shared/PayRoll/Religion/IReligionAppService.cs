using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Dto;
using ERP.PayRoll.Religion.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.PayRoll.Religion
{
    public interface IReligionAppService : IApplicationService
    {
        Task<PagedResultDto<GetReligionForViewDto>> GetAll(GetAllReligionInput input);

        Task<GetReligionForViewDto> GetReligionForView(int id);

        Task<GetReligionForEditOutput> GetReligionForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditReligionDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetReligionToExcel(GetAllReligionForExcelInput input);
    }
}
