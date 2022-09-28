using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms
{
    public interface IGroupCategoriesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetGroupCategoryForViewDto>> GetAll(GetAllGroupCategoriesInput input);

        Task<GetGroupCategoryForViewDto> GetGroupCategoryForView(int id);

		Task<GetGroupCategoryForEditOutput> GetGroupCategoryForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditGroupCategoryDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetGroupCategoriesToExcel(GetAllGroupCategoriesForExcelInput input);

		
    }
}