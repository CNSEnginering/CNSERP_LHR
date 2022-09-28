using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Dto;
using ERP.PayRoll.Grades.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.PayRoll.Grades
{
    public interface IGradeAppService : IApplicationService
    {
        Task<PagedResultDto<GetGradeForViewDto>> GetAll(GetAllGradeInput input);

        Task<GetGradeForViewDto> GetGradeForView(int id);

        Task<GetGradeForEditOutput> GetGradeForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditGradeDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetGradeToExcel(GetAllGradeForExcelInput input);
    }
}
