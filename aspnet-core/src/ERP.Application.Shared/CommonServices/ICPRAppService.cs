using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Dto;
using ERP.CommonServices.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.CommonServices
{
    public interface ICPRAppService : IApplicationService
    {
        Task<PagedResultDto<GetCPRForViewDto>> GetAll(GetAllCPRInput input);

        Task<GetCPRForViewDto> GetCPRForView(int id);

        Task<GetCPRForEditOutput> GetCPRForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCPRDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetCPRToExcel(GetAllCPRForExcelInput input);
    }
}
