using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using System.Collections.Generic;

namespace ERP.GeneralLedger.SetupForms
{
    public interface IGroupCodesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetGroupCodeForViewDto>> GetAll(GetAllGroupCodesInput input);

        Task<GetGroupCodeForViewDto> GetGroupCodeForView(int id);

		Task<GetGroupCodeForEditOutput> GetGroupCodeForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditGroupCodeDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetGroupCodesToExcel(GetAllGroupCodesForExcelInput input);

		
		Task<ListResultDto<GroupCategoryForComboboxDto>> GetGroupCategoryForCombobox();
		
    }
}