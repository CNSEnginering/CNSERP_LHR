using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.SubDesignations.Dtos;
using ERP.Dto;


namespace ERP.PayRoll.SubDesignations
{
    public interface ISubDesignationsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetSubDesignationsForViewDto>> GetAll(GetAllSubDesignationsInput input);

        Task<GetSubDesignationsForViewDto> GetSubDesignationsForView(int id);

		Task<GetSubDesignationsForEditOutput> GetSubDesignationsForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditSubDesignationsDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetSubDesignationsToExcel(GetAllSubDesignationsForExcelInput input);

		
    }
}