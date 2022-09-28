using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.DeductionTypes.Dtos;
using ERP.Dto;


namespace ERP.PayRoll.DeductionTypes
{
    public interface IDeductionTypesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDeductionTypesForViewDto>> GetAll(GetAllDeductionTypesInput input);

        Task<GetDeductionTypesForViewDto> GetDeductionTypesForView(int id);

		Task<GetDeductionTypesForEditOutput> GetDeductionTypesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditDeductionTypesDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetDeductionTypesToExcel(GetAllDeductionTypesForExcelInput input);

		
    }
}