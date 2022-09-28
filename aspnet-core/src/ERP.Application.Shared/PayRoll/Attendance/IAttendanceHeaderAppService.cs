using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Dto;
using ERP.PayRoll.Attendance.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.PayRoll.Attendance
{
    public interface IAttendanceHeaderAppService : IApplicationService
    {
        Task<PagedResultDto<GetAttendanceHeaderForViewDto>> GetAll(GetAllAttendanceHeaderInput input);

        Task<GetAttendanceHeaderForEditOutput> GetAttendanceHeaderForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAttendanceHeaderDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetAttendanceHeaderToExcel(GetAllAttendanceHeaderForExcelInput input);

    }
}
