using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.EmployeeEarnings.Dtos;
using ERP.Dto;
using System.Collections.Generic;

namespace ERP.PayRoll.EmployeeEarnings
{
    public interface IEmployeeEarningsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEmployeeEarningsForViewDto>> GetAll(GetAllEmployeeEarningsInput input);

		Task<GetEmployeeEarningsForEditOutput> GetEmployeeEarningsForEdit(int ID);

		Task CreateOrEdit(ICollection<CreateOrEditEmployeeEarningsDto> input);

		Task Delete(int ID);

		Task<FileDto> GetEmployeeEarningsToExcel(GetAllEmployeeEarningsForExcelInput input);

		
    }
}