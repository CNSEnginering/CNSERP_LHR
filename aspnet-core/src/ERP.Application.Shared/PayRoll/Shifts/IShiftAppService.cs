using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Dto;
using ERP.PayRoll.Shifts.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.PayRoll.Shifts
{
    public interface IShiftAppService : IApplicationService
    {
        Task<PagedResultDto<GetShiftForViewDto>> GetAll(GetAllShiftInput input);

        Task<GetShiftForViewDto> GetShiftForView(int id);

        Task<GetShiftForEditOutput> GetShiftForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditShiftDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetShiftToExcel(GetAllShiftForExcelInput input);
    }
}
