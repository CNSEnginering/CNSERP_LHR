using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.AccountPayables.Dtos;
using ERP.Dto;

namespace ERP.AccountPayables
{
    public interface IAPTermsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAPTermForViewDto>> GetAll(GetAllAPTermsInput input);

        Task<GetAPTermForViewDto> GetAPTermForView(int id);

		Task<GetAPTermForEditOutput> GetAPTermForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditAPTermDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetAPTermsToExcel(GetAllAPTermsForExcelInput input);

		
    }
}