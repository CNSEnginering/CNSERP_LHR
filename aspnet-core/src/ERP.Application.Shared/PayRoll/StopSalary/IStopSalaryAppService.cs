using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.StopSalary.Dtos;
using ERP.Dto;
using System.Collections.Generic;

namespace ERP.PayRoll.StopSalary
{
    public interface IStopSalaryAppService : IApplicationService 
    {
        Task<PagedResultDto<GetStopSalaryForViewDto>> GetAll(GetAllStopSalaryInput input);

        Task<GetStopSalaryForViewDto> GetStopSalaryForView(int id);

		Task<GetStopSalaryForEditOutput> GetStopSalaryForEdit(EntityDto input);

		Task CreateOrEdit(List<CreateOrEditStopSalaryDto> input);

		Task Delete(EntityDto input);

		
    }
}