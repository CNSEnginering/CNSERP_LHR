using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.EarningTypes.Dtos;
using ERP.Dto;


namespace ERP.PayRoll.EarningTypes
{
    public interface IEarningTypesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEarningTypesForViewDto>> GetAll(GetAllEarningTypesInput input);

        Task<GetEarningTypesForViewDto> GetEarningTypesForView(int id);

		Task<GetEarningTypesForEditOutput> GetEarningTypesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEarningTypesDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetEarningTypesToExcel(GetAllEarningTypesForExcelInput input);

		
    }
}