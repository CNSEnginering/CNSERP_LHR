using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Dto;
using ERP.PayRoll.Attendance.Dtos;

namespace ERP.PayRoll.AttendanceDetail
{
    public interface IAttendanceDetailAppService : IApplicationService
    {
        Task<PagedResultDto<GetAttendanceDetailForViewDto>> GetAll(GetAllAttendanceDetailInput input);

        Task<GetAttendanceDetailForEditOutput> GetAttendanceDetailForEdit(int ID);

        Task CreateOrEdit(ICollection<CreateOrEditAttendanceDetailDto> input);

        Task Delete(int input);

        Task<FileDto> GetAttendanceDetailToExcel(GetAllAttendanceDetailForExcelInput input);
    }
}