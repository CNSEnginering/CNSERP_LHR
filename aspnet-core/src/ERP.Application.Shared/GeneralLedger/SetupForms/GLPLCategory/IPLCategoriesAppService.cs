using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.GLPLCategory.Dtos;
using ERP.Dto;


namespace ERP.GeneralLedger.SetupForms.GLPLCategory
{
    public interface IPLCategoriesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPLCategoryForViewDto>> GetAll(GetAllPLCategoriesInput input);

        Task<GetPLCategoryForViewDto> GetPLCategoryForView(int id);

		Task<GetPLCategoryForEditOutput> GetPLCategoryForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditPLCategoryDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPLCategoriesToExcel(GetAllPLCategoriesForExcelInput input);

		
    }
}