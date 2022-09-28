using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Dto;
using ERP.PayRoll.Designation.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.PayRoll.Designation
{
    public interface IDesignationAppService : IApplicationService
    {
        Task<PagedResultDto<GetDesignationForViewDto>> GetAll(GetAllDesignationInput input);

        Task<GetDesignationForViewDto> GetDesignationForView(int id);

        Task<GetDesignationForEditOutput> GetDesignationForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditDesignationDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetDesignationToExcel(GetAllDesignationForExcelInput input);
    }
}
